using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IWmeterService
    {
        Task<List<WmeterDTO>> List();
        Task<WmeterDTO> Get(int id);
        Task<WmeterDTO> Create(WmeterDTO model);
        Task<bool> Edit(WmeterDTO model);
        Task<bool> Delete(int id);
    }
}
