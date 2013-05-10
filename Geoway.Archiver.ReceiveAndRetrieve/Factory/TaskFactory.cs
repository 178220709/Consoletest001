using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumTaskType"></param>
        /// <returns></returns>
        public static Task CreateTask(EnumTaskType enumTaskType)
        {
            Task task = null;
            switch (enumTaskType)
            {
                case EnumTaskType.Upload:
                    task = new UpLoadTask();
                    break;
                case EnumTaskType.Download:
                    // qfc
                    //task = new DownLoadTask();
                    break;
            }
            return task;
        }
 
    }
}