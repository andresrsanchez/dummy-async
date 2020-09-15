using System.Threading.Tasks;

namespace async
{
    public class ValueTasks
    {
        static int? cache = null;

        public static async Task<int> demo_task()
        {
            if (cache == null)
            {
                return await get_result();
            }

            return cache.Value;
        }

        public static ValueTask<int> demo_valuetask()
        {
            if (cache == null)
            {
                return new ValueTask<int>(get_result());
            }

            return new ValueTask<int>(cache.Value);
        }

        static async Task<int> get_result()
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(1000);
                return 1;
            });
        }
    }
}
