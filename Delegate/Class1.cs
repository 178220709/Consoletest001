using System;

namespace Consoletest001.Delegate
{
    // ��ˮ��
    public class Heater
    {
        private int _temperature;
        public string Type = "RealFire 001"; // ����ͺ���Ϊ��ʾ
        public string Area = "China Xian"; // ��Ӳ�����Ϊ��ʾ
        //����ί��
        public delegate void BoiledEventHandler(Object sender, BoiledEventArgs e);

        public event BoiledEventHandler Boiled; //�����¼�

        // ����BoiledEventArgs�࣬���ݸ�Observer������Ȥ����Ϣ
        public class BoiledEventArgs : EventArgs
        {
            public readonly int temperature;

            public BoiledEventArgs(int temperature)
            {
                this.temperature = temperature;
            }
        }

        // ���Թ��̳��� Heater ������д���Ա�̳���ܾ�������������ļ���
        protected virtual void OnBoiled(BoiledEventArgs e)
        {
            if (Boiled != null)
            {
                // ����ж���ע��
                Boiled(this, e); // ��������ע�����ķ���
            }
        }

        // ��ˮ��
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                _temperature = i;
                if (_temperature > 95)
                {
                    //����BoiledEventArgs ����
                    BoiledEventArgs e = new BoiledEventArgs(_temperature);
                    OnBoiled(e); // ���� OnBolied����
                }
            }
        }
    }

    // ������
    public class Alarm
    {
        public void MakeAlert(Object sender, Heater.BoiledEventArgs e)
        {
            Heater heater = (Heater) sender; //�����ǲ��Ǻ���Ϥ�أ�
            //���� sender �еĹ����ֶ�
            Console.WriteLine("Alarm��{0} - {1}: ", heater.Area, heater.Type);
            Console.WriteLine("Alarm: �����֣�ˮ�Ѿ� {0} ���ˣ�", e.temperature);
            Console.WriteLine();
        }
    }

    // ��ʾ��
    public class Display
    {
        public static void ShowMsg(Object sender, Heater.BoiledEventArgs e)
        {
            //��̬����
            Heater heater = (Heater) sender;
            Console.WriteLine("Display��{0} - {1}: ", heater.Area, heater.Type);
            Console.WriteLine("Display��ˮ���տ��ˣ���ǰ�¶ȣ�{0}�ȡ�", e.temperature);
            Console.WriteLine();
        }
    }

    public  class Programtest
    {
        public static void Mainsdf()
        {
            Heater heater = new Heater();
            Alarm alarm = new Alarm();
            heater.Boiled += alarm.MakeAlert; //ע�᷽��
            heater.Boiled += (new Alarm()).MakeAlert; //����������ע�᷽��
            heater.Boiled += alarm.MakeAlert; //Ҳ������ôע��
            heater.Boiled += Display.ShowMsg; //ע�ᾲ̬����
            heater.BoilWater(); //��ˮ�����Զ�����ע�������ķ���
        }
    }
}
