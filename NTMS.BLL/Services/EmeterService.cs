using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class EmeterService : IEmeterService
    {
        private readonly IGenericRepository<Emeter> _emeterRepository;
        private readonly IMapper _mapper;

        public EmeterService(IGenericRepository<Emeter> emeterRepository, IMapper mapper)
        {
            _emeterRepository = emeterRepository;
            _mapper = mapper;
        }
        public async Task<List<EmeterDTO>> List()
        {
            try
            {
                var query = await _emeterRepository.GetAll();
                var meterList = query.Include(m => m.Ereadings).Include(m => m.Flat).ToList();
                //      var meterList = query.Include(m => m.Flat).Where(m=>m.IsActive==true);
                var list = _mapper.Map<List<EmeterDTO>>(meterList);
                return list;
            }
            catch { throw; }
        }
        public async Task<EmeterDTO> Get(int id)
        {

            try
            {

                var meter = await _emeterRepository.GetAll(m => m.Id == id);
                var emeter = meter.Include(m => m.Flat).First();
                return _mapper.Map<EmeterDTO>(emeter);
            }
            catch { throw; }
        }



        public async Task<EmeterDTO> Create(EmeterDTO model)
        {
            try
            {
                var meter = await _emeterRepository.Create(_mapper.Map<Emeter>(model));
                if (meter.Id == 0) throw new TaskCanceledException("Failed to add new meter");

                var query = await _emeterRepository.GetAll(m => m.Id == meter.Id);
                //          meter = query.Include(m => m.Ereadings).First();

                return _mapper.Map<EmeterDTO>(query);
            }
            catch { throw; }
        }



        public async Task<bool> Edit(EmeterDTO model)
        {
            try
            {
                var meterModel = _mapper.Map<Emeter>(model);
                var meter = await _emeterRepository.Get(m => m.Id == meterModel.Id);
                if (meter == null) throw new TaskCanceledException("Meter not exists");

                meter.MeterNumber = meterModel.MeterNumber;
                meter.FlatId = meterModel.FlatId;
                meter.IsActive = meterModel.IsActive;

                bool request = await _emeterRepository.Edit(meter);
                if (!request) throw new TaskCanceledException("Failed to edit meter");
                return request;
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var meter = await _emeterRepository.Get(m => m.Id == id);
                if (meter == null) throw new TaskCanceledException("Meter not exists");

                bool request = await _emeterRepository.Delete(meter);
                if (!request) throw new TaskCanceledException("Failed to delete meter");
                return request;
            }
            catch { throw; }
        }
    }
}
