using System.Threading.Tasks;

namespace BaseFeatureDemo.awaitDemo
{
    /*
     * 演示一个简单示例，其中一个方法发生阻塞，等待 async 方法的结果。
     * 此代码仅在控制台应用程序中工作良好，但是在从 GUI 或 ASP.NET 上下文调用时会死锁。
     * 此行为可能会令人困惑，尤其是通过调试程序单步执行时，这意味着没完没了的等待。 
     * 在调用 Task.Wait 时，导致死锁的实际原因在调用堆栈中上移。在异步代码上阻塞时的常见死锁问题
     * **/

    public class AsyncTaskDemo3
    {
        public static  void Main1()
        {
            DeadlockDemo.Test();
        }
    }



    public static class DeadlockDemo
    {
        private static async Task DelayAsync()
        {
            await Task.Delay(1000);
        }
        // This method causes a deadlock when called in a GUI or ASP.NET context.
        public static void Test()
        {
            // Start the delay.
            var delayTask = DelayAsync();
            // Wait for the delay to complete.
            delayTask.Wait();
        }
    }

    /*
     这种死锁的根本原因是 await 处理上下文的方式。 默认情况下，当等待未完成的 Task 时，会捕获当前“上下文”，在 Task 完成时使用该上下文恢复方法的执行。 此“上下文”是当前 SynchronizationContext（除非它是 null，这种情况下则为当前 TaskScheduler）。 GUI 和 ASP.NET 应用程序具有 SynchronizationContext，它每次仅允许一个代码区块运行。 当 await 完成时，它会尝试在捕获的上下文中执行 async 方法的剩余部分。 但是该上下文已含有一个线程，该线程在（同步）等待 async 方法完成。 它们相互等待对方，从而导致死锁。

请注意，控制台应用程序不会形成这种死锁。 它们具有线程池 SynchronizationContext 而不是每次执行一个区块的 SynchronizationContext，因此当 await 完成时，它会在线程池线程上安排 async 方法的剩余部分。 该方法能够完成，并完成其返回任务，因此不存在死锁。 当程序员编写测试控制台程序，观察到部分异步代码按预期方式工作，然后将相同代码移动到 GUI 或 ASP.NET 应用程序中会发生死锁，此行为差异可能会令人困惑。

此问题的最佳解决方案是允许异步代码通过基本代码自然扩展。 如果采用此解决方案，则会看到异步代码扩展到其入口点（通常是事件处理程序或控制器操作）。 控制台应用程序不能完全采用此解决方案，因为 Main 方法不能是 async。 如果 Main 方法是 async，则可能会在完成之前返回，从而导致程序结束。 图 4 演示了指导原则的这一例外情况： 控制台应用程序的 Main 方法是代码可以在异步方法上阻塞为数不多的几种情况之一。
     
     
     */

}