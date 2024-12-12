using AutoMapper;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class EruleService : IEruleService
    {
        private readonly IGenericRepository<EbillingRule> _eruleRepo;
        private readonly IMapper _mapper;

        public EruleService(IGenericRepository<EbillingRule> eruleRepo, IMapper mapper)
        {
            _eruleRepo = eruleRepo;
            _mapper = mapper;
        }
        public async Task<EbillingRuleDTO> Get()
        {
            try
            {
                var query = await _eruleRepo.GetAll();
                var billingRule = query.OrderBy(r => r.Id).LastOrDefault();
                return _mapper.Map<EbillingRuleDTO>(billingRule);
            }
            catch { throw; }
        }
        public async Task<EbillingRuleDTO> Create(EbillingRuleDTO model)
        {
            try
            {

                var rules = await _eruleRepo.Create(_mapper.Map<EbillingRule>(model));
                if (rules.Id == 0) throw new TaskCanceledException("Failed to add new electric rule");
                var rule = await _eruleRepo.Get(r => r.Id == rules.Id);
                return _mapper.Map<EbillingRuleDTO>(rule);
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var rule = await _eruleRepo.Get(r => r.Id == id);
                if (rule == null) throw new TaskCanceledException("Billing rule not exists");

                bool request = await _eruleRepo.Delete(rule);
                if (!request) throw new TaskCanceledException("Failed to delete Billing rule");
                return request;
            }
            catch { throw; }
        }

        public async Task<bool> Edit(EbillingRuleDTO model)
        {
            try
            {

                var ruleModel = _mapper.Map<EbillingRule>(model);
                var rule = await _eruleRepo.Get(r => r.Id == ruleModel.Id);
                if (rule == null) throw new TaskCanceledException("Electric billing rules not exists");

                rule.From1 = ruleModel.From1;
                rule.From2 = ruleModel.From2;

                rule.From3 = ruleModel.From3;
                rule.From4 = ruleModel.From4;
                rule.To1 = ruleModel.To1;
                rule.To2 = ruleModel.To2;
                rule.To3 = ruleModel.To3;
                rule.To4 = ruleModel.To4;
                rule.Rate1 = ruleModel.Rate1;
                rule.Rate2 = ruleModel.Rate2;
                rule.Rate3 = ruleModel.Rate3;
                rule.Rate4 = ruleModel.Rate4;
                rule.DemandCharge = ruleModel.DemandCharge;
                rule.CommercialRate = ruleModel.CommercialRate;
                rule.CommercialDc = ruleModel.CommercialDc;
                rule.MeterRent = ruleModel.MeterRent;
                rule.Vat = ruleModel.Vat;

                bool request = await _eruleRepo.Edit(rule);
                if (!request) throw new TaskCanceledException("Failed to edit Electric billing rules");
                return request;
            }
            catch { throw; }
        }

    }
}
