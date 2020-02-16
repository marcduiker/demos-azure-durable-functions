using System.Collections.Generic;
using System.Threading.Tasks;

namespace DurableFunctions.Demo.ScreenShots.Interfaces
{
    public interface IService
    {
        Task<IEnumerable<string>> GetDetailsAsync(string name);
    }
}