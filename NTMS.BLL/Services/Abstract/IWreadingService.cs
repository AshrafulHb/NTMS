using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IWreadingService
    {
        Task<WreadingDTO> LoadReading(int meterId, string firstDate, string lastDate);
        Task<WreadingDTO> Create(WreadingDTO model);
        Task<bool> Edit(WreadingDTO model);
        Task<bool> Delete(int id);
        Task<WreadingDTO> GetLastReading(int meterId);
    }
}
