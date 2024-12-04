using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class WmeterService : IWmeterService
    {
        private readonly IGenericRepository<Wmeter> _wMetrRepository;
        private readonly IMapper _mapper;

        public WmeterService(IGenericRepository<Wmeter> wMetrRepository, IMapper mapper)
        {
            _wMetrRepository = wMetrRepository;
            _mapper = mapper;
        }
        public async Task<List<WmeterDTO>> List()
        {
            try
            {
                var query = await _wMetrRepository.GetAll();
                var meterList = query.Include(m => m.Flat).ToList();

                return _mapper.Map<List<WmeterDTO>>(meterList);
            }
            catch { throw; }
        }
        public async Task<WmeterDTO> Get(int id)
        {
            try
            {
                var meter = await _wMetrRepository.GetAll(m => m.Id == id);
                var wMeter = meter.Include((m) => m.Flat).First();
                return _mapper.Map<WmeterDTO>(wMeter);
            }
            catch { throw; }
        }
        public async Task<bool> Edit(WmeterDTO model)
        {

            try
            {
                var meterModel = _mapper.Map<Wmeter>(model);
                var meter = await _wMetrRepository.Get(m => m.Id == meterModel.Id);
                if (meter == null) throw new TaskCanceledException("Meter not exists");

                meter.MeterNumber = meterModel.MeterNumber;
                meter.FlatId = meterModel.FlatId;
                meter.IsActive = meterModel.IsActive;

                bool request = await _wMetrRepository.Edit(meter);
                if (!request) throw new TaskCanceledException("Failed to edit meter");
                return request;
            }
            catch { throw; }
        }

        public async Task<WmeterDTO> Create(WmeterDTO model)
        {

            try
            {
                var wMeter = await _wMetrRepository.Create(_mapper.Map<Wmeter>(model));
                if (wMeter.Id == 0) throw new TaskCanceledException("Failed to add new meter");
                var query = await _wMetrRepository.GetAll(m => m.Id == wMeter.Id);
                return _mapper.Map<WmeterDTO>(query);
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {

            try
            {

                var meter = await _wMetrRepository.Get(m => m.Id == id);
                if (meter == null) throw new TaskCanceledException("Meter not exists");

                bool request = await _wMetrRepository.Delete(meter);
                if (!request) throw new TaskCanceledException("Failed to delete meter");
                return request;
            }
            catch { throw; }
        }





    }
}
