using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace async
{
    public class ThreadPools
    {
        public static void demo()
        {
            Console.Clear();

            ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            Console.WriteLine($"Worker threads (min: {minWorkerThreads}, max: {maxWorkerThreads})");
            Console.WriteLine($"IOCP threads: (min: {minCompletionPortThreads}, max: {maxCompletionPortThreads})");

            var statsThread = new Thread(PrintThreadPoolStats)
            {
                IsBackground = true
            };
            statsThread.Start();

            var spawningThread = new Thread(SpawnWork)
            {
                IsBackground = true
            };
            spawningThread.Start();
            Console.ReadLine();
        }

        static void SpawnWork()
        {
            while (true)
            {
                Thread.Sleep(100);
                ThreadPool.QueueUserWorkItem(DoWork);
            }
        }

        static void DoWork(object? arg)
        {
            Console.CursorTop = 3;
            Console.Write(".");
            Thread.Sleep(60_00);
        }

        static void PrintThreadPoolStats()
        {
            while (true)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 2;
                ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);
                Console.WriteLine($"Current: {ThreadPool.ThreadCount}, Queued: {ThreadPool.PendingWorkItemCount}, Done: {ThreadPool.CompletedWorkItemCount}, Worker: {workerThreads}, IOCP: {completionPortThreads}");
                Thread.Sleep(1000);
            }
        }
    }
}
