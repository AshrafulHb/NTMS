using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IFlatService
    {

        Task<List<FlatDTO>> List();
        Task<FlatDTO> Create(FlatDTO model);
        Task<bool> Edit(FlatDTO model);
        Task<bool> Delete(int id);
    }
}
