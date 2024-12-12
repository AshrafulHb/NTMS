using AutoMapper;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class WruleService : IWruleService
    {
        private readonly IGenericRepository<WbillingRule> _wRulesRepository;
        private readonly IMapper _mapper;

        public WruleService(IGenericRepository<WbillingRule> wRulesRepository, IMapper mapper)
        {
            _wRulesRepository = wRulesRepository;
            _mapper = mapper;
        }
        public async Task<WbillingRuleDTO> Get()
        {
            try
            {
                var query = await _wRulesRepository.GetAll();
                var billingRule = query.OrderBy(r => r.Id).LastOrDefault();
                return _mapper.Map<WbillingRuleDTO>(billingRule);
            }
            catch { throw; }
        }
        public async Task<WbillingRuleDTO> Create(WbillingRuleDTO model)
        {
            try
            {

                var rules = await _wRulesRepository.Create(_mapper.Map<WbillingRule>(model));
                if (rules.Id == 0) throw new TaskCanceledException("Failed to add new water rule");
                var rule = await _wRulesRepository.Get(r => r.Id == rules.Id);
                return _mapper.Map<WbillingRuleDTO>(rule);
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var rule = await _wRulesRepository.Get(r => r.Id == id);
                if (rule == null) throw new TaskCanceledException("Billing rule not exists");

                bool request = await _wRulesRepository.Delete(rule);
                if (!request) throw new TaskCanceledException("Failed to delete Billing rule");
                return request;
            }
            catch { throw; }
        }

        public async Task<bool> Edit(WbillingRuleDTO model)
        {
            try
            {

                var ruleModel = _mapper.Map<WbillingRule>(model);
                var rule = await _wRulesRepository.Get(r => r.Id == ruleModel.Id);
                if (rule == null) throw new TaskCanceledException("Water billing rules not exists");


                rule.UnitPrice = ruleModel.UnitPrice;
                rule.Vat = ruleModel.Vat;
                rule.ServiceCharge = ruleModel.ServiceCharge;

                bool request = await _wRulesRepository.Edit(rule);
                if (!request) throw new TaskCanceledException("Failed to edit Water billing rules");
                return request;
            }
            catch { throw; }
        }
    }
}
