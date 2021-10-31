using System.Threading.Tasks;

namespace Business.Services
{
    public class TaskProxy : ITaskProxy
    {
        public async Task Delay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
    }
}