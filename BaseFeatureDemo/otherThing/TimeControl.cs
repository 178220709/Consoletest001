using System;
using System.Threading;

namespace BaseFeatureDemo.otherThing
{
    public class TimeControl
    {

        public static void mainsdf()
        {
            DateTime dt = DateTime.Parse(" 23:59:59");
            TimeSpan ts = dt - DateTime.Now;

            bool bbb = (dt > DateTime.Now);
            Console.WriteLine(ts.TotalSeconds);
        }

        public TimeControl()
        {

        }
    }


    public static class StateChecker
    {
        public static void GetCurrentState(StateModel model)
        {
            int state = 3; //判断状态的逻辑。。。耗时10s

            model.State = state;
            if (model.CallAction != null)
            {
                model.CallAction.Invoke(model);
            }
        }
    }
    public class StateModel
    {
        public int  State { get; set; }
        public Action<StateModel> CallAction { get; set; } 
    }

    public class StateMonito
    {
        /// <summary>
        /// 这个函数会运行在多线程环境中
        /// </summary>
        public void CheckState()
        {
            var stateModel = new StateModel();
            //局部变量，线程安全，每个线程都有属于自己有效域的stateModel
            stateModel.CallAction = model =>
            {
                //doSomeThing 如果逻辑比较通用，可以在StateChecker里面封装一下
            };
            StateChecker.GetCurrentState(stateModel);//这里会阻塞10s，之后会调用stateModel.CallAction，完成后续逻辑
        }
    }

}