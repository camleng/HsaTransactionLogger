using System.Threading.Tasks;

namespace Business.Services
{
    public interface ITaskProxy
    {
        Task Delay(int milliseconds);
    }
}