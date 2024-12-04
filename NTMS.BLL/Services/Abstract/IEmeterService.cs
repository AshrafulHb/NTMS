using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IEmeterService
    {
        Task<List<EmeterDTO>> List();
        Task<EmeterDTO> Get(int id);
        Task<EmeterDTO> Create(EmeterDTO model);
        Task<bool> Edit(EmeterDTO model);
        Task<bool> Delete(int id);
    }
}
