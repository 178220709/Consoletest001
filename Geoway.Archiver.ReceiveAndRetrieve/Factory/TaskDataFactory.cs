using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskDataFactory
    {
        public static TaskData CreateTaskData(EnumTaskType enumTaskType)
        {
            TaskData taskData = null;
            switch (enumTaskType)
            {
                case EnumTaskType.Upload:
                    taskData = new UpLoadTaskData();
                    break;
                case EnumTaskType.Download:
                    // qfc
                    //taskData = new DownLoadTaskData();
                    break;
            }
            return taskData;
        }
    }
}
