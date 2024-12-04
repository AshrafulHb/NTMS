using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface ITenantService
    {
        Task<List<TenantDTO>> List();
        Task<TenantDTO> Create(TenantDTO model);
        Task<bool> Edit(TenantDTO model);
        Task<bool> Delete(int id);
    }
}
