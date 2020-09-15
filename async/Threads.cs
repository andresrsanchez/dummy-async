using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace async
{
    public class Threads
    {
        public static async Task demo_await_async()
        {
            var task1 = Task.Run(() => DoWork(30000000));
            var task2 = Task.Run(() => DoWork(60000000));
            var task3 = Task.Run(() => DoWork(90000000));

            await Task.WhenAll(task1, task2, task3);
        }

        public static Task demo_sync_async()
        {
            Task.Run(() => DoWork(30000000)).Wait();
            Task.Run(() => DoWork(60000000)).Wait();
            Task.Run(() => DoWork(90000000)).Wait();

            return Task.CompletedTask;
        }

        public static void demo_sync()
        {
            DoWork(30000000);
            DoWork(60000000);
            DoWork(90000000);
        }

        static void DoWork(int numLoops)
        {
            while (numLoops >= 0)
            {
                numLoops--;
            }
        }
    }
}
