using NTMS.DTO;

namespace NTMS.BLL.Services.Abstract
{
    public interface IWruleService
    {
        Task<bool> Edit(WbillingRuleDTO model);
        Task<WbillingRuleDTO> Get();
        Task<WbillingRuleDTO> Create(WbillingRuleDTO model);
        Task<bool> Delete(int id);
    }
}
