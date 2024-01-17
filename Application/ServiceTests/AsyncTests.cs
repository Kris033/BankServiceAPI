using Xunit;

namespace ServiceTests
{
    public class AsyncTests
    {
        [Fact]
        public async void TestAsync()
        {
            Random rnd = new Random();
            ThreadPool.SetMaxThreads(10, 10);
            Task[] tasks = new Task[15];
            for (int i = 0; i < tasks.Length; i++)
            {
                while (ThreadPool.ThreadCount == 10)
                {
                    Console.WriteLine("Ждём пока не закончит работу одна из задач...");
                    Thread.Sleep(5000);
                }
                tasks[i] = new Task(() =>
                {
                    Console.WriteLine($"Задача №{i} начала работу в пуле {Task.CurrentId}");
                    Thread.Sleep(rnd.Next(1, 21) * 1000);
                    Console.WriteLine($"Закончил работать пул {Task.CurrentId}");
                });
                tasks[i].Start();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Ждём пока все задачи будут закончены");
            await Task.WhenAll(tasks);
            Console.WriteLine("Все задачи успешно завершены");
            //Assert
            Assert.DoesNotContain(tasks, task => !task.IsCompleted);
            
        }
    }
}
