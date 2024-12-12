using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IEruleService
    {
        Task<bool> Edit(EbillingRuleDTO model);
        Task<EbillingRuleDTO> Get();
        Task<EbillingRuleDTO> Create(EbillingRuleDTO model);
        Task<bool> Delete(int id);
    }
}
