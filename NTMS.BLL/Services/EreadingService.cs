using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;
using System.Globalization;

namespace NTMS.BLL.Services
{
    public class EreadingService(IGenericRepository<Ereading> ereadingRepository, IMapper mapper) : IEreadingService
    {
        private readonly IGenericRepository<Ereading> _ereadingRepository = ereadingRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<EreadingDTO> LoadReading(int meterId, string firstDate, string lastDate)
        {
            DateTime fDate = DateTime.ParseExact(firstDate, "dd/MM/yyyy", new CultureInfo("en-US"));
            DateTime lDate = DateTime.ParseExact(lastDate, "dd/MM/yyyy", new CultureInfo("en-US"));
            try
            {
                var query = await _ereadingRepository.GetAll(r => r.EmeterId == meterId && ((r.StartDate >= fDate && r.StartDate <= lDate) || (r.EndDate >= fDate && r.EndDate <= lDate)));
                var reading = query.Include(r => r.Emeter).FirstOrDefault();
                if (reading == null) throw new TaskCanceledException("No reading found");

                return _mapper.Map<EreadingDTO>(reading);
            }
            catch { throw; }
        }

        public async Task<EreadingDTO> Create(EreadingDTO model)
        {

            try
            {
                var _reading = _mapper.Map<Ereading>(model);
                if (_reading.StartDate >= _reading.EndDate) throw new TaskCanceledException("End date must be later than start date!");
                else if (_reading.PreviousReading > _reading.CurrentReading) throw new TaskCanceledException("Current reading can not be less than previous reading!");

                var reading = await _ereadingRepository.Create(_reading);
                if (reading.Id == 0) throw new TaskCanceledException("Failed to add new reading");
                return _mapper.Map<EreadingDTO>(reading);
            }
            catch { throw; }
        }

        public async Task<bool> Edit(EreadingDTO model)
        {

            try
            {
                var readingModel = _mapper.Map<Ereading>(model);
                var reading = await _ereadingRepository.Get(r => r.Id == model.Id) ?? throw new TaskCanceledException("reading not found");
                reading.StartDate = readingModel.StartDate;
                reading.EndDate = readingModel.EndDate;
                reading.PreviousReading = readingModel.PreviousReading;
                reading.CurrentReading = readingModel.CurrentReading;
                reading.EmeterId = model.EmeterId;

                if (reading.StartDate == reading.EndDate || reading.StartDate > reading.EndDate) throw new TaskCanceledException("End date must be later than start date!");
                else if (reading.PreviousReading > reading.CurrentReading) throw new TaskCanceledException("Current reading cand be less than previous reading!");


                bool request = await _ereadingRepository.Edit(reading);
                if (!request) throw new TaskCanceledException("Failed to edit  reading");
                return request;
            }
            catch { throw; }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {

                var reading = await _ereadingRepository.Get(r => r.Id == id);
                if (reading == null) throw new TaskCanceledException("reading not found");
                bool request = await _ereadingRepository.Delete(reading);
                if (!request) throw new TaskCanceledException("Failed to delete  reading");
                return request;
            }
            catch { throw; }
        }
        public async Task<EreadingDTO> GetLastReading(int meterId)
        {
            try
            {
                var query = await _ereadingRepository.GetAll(r => r.EmeterId == meterId);
                var reading = query.OrderBy(r => r.CurrentReading).LastOrDefault();
                if (reading == null) throw new TaskCanceledException("No reading found");

                return _mapper.Map<EreadingDTO>(reading);
            }
            catch { throw; }
        }

    }
}
