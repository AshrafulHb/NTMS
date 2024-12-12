using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class TenantService : ITenantService
    {
        private readonly IGenericRepository<Tenant> _tenantRepository;
        private readonly IMapper _mapper;

        public TenantService(IGenericRepository<Tenant> tenantRepository, IMapper mapper)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
        }
        public async Task<List<TenantDTO>> List()
        {
            try
            {
                var tenantQuery = await _tenantRepository.GetAll();
                var tenantList = tenantQuery.Include(t => t.Flat).ToList();
                return _mapper.Map<List<TenantDTO>>(tenantList);
            }
            catch { throw; }
        }
        public async Task<TenantDTO> Create(TenantDTO model)
        {
            try
            {
                var tenant = await _tenantRepository.Create(_mapper.Map<Tenant>(model));
                if (tenant.Id == 0) throw new TaskCanceledException("Failed to add tenant");
                var query = await _tenantRepository.GetAll(t => t.Id == tenant.Id);

                tenant = query.Include(t => t.Flat).First();
                return _mapper.Map<TenantDTO>(tenant);
            }
            catch { throw; }

        }

        public async Task<bool> Edit(TenantDTO model)
        {
            try
            {
                var tenantModel = _mapper.Map<Tenant>(model);
                var tenant= await _tenantRepository.Get(t=>t.Id == tenantModel.Id);
                if (tenant == null) throw new TaskCanceledException("Tenant not exists");

                tenant.StartDate=tenantModel.StartDate;
                tenant.Name=tenantModel.Name;
                tenant.Paddress=tenantModel.Paddress;
                tenant.Occupation=tenantModel.Occupation;
                tenant.Telephone=tenantModel.Telephone;
                tenant.SecurityDeposit = tenantModel.SecurityDeposit;
                tenant.FlatId=tenantModel.FlatId;
                tenant.IsActive=tenantModel.IsActive;

                bool request = await _tenantRepository.Edit(tenant);
                if (!request) throw new TaskCanceledException("Failed to edit Tenant");
                return request;
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var tenant = await _tenantRepository.Get(t=>t.Id == id);
                if (tenant == null) throw new TaskCanceledException("Tenent no found");

                bool request = await _tenantRepository.Delete(tenant);
                if (!request) throw new TaskCanceledException("Failed to delete tenant");
                return request;
            }
            catch { throw; }

        }
    }
}