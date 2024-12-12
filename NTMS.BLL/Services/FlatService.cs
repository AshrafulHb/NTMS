using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.Repository.Abstract;
using NTMS.DTO;
using NTMS.Model;

namespace NTMS.BLL.Services
{
    public class FlatService : IFlatService
    {
        private readonly IGenericRepository<Flat> _flatRepository;
        private readonly IMapper _mapper;

        public FlatService(IGenericRepository<Flat> flatRepository, IMapper mapper)
        {
            _flatRepository = flatRepository;
            _mapper = mapper;
        }
        public async Task<List<FlatDTO>> List()
        {
            try
            {
                var flatQuery = await _flatRepository.GetAll();
            //    var flatList = flatQuery.Include(f => f.Tenants).Include(f => f.Emeters).ToList();
                return _mapper.Map<List<FlatDTO>>(flatQuery);
            }
            catch { throw; }
        }
        public async Task<FlatDTO> Create(FlatDTO model)
        {
            try
            {
                var flat = await _flatRepository.Create(_mapper.Map<Flat>(model));
                if (flat.Id == 0) throw new TaskCanceledException("Failed to add new flat");

                var query = await _flatRepository.GetAll(f => f.Id == flat.Id);
             //   flat = query.Include(f => f.Tenants).Include(f => f.Emeters).First();

                return _mapper.Map<FlatDTO>(query);
            }
            catch { throw; }
        }


        public async Task<bool> Edit(FlatDTO model)
        {
            try
            {
                var flatModel = _mapper.Map<Flat>(model);
                var flat = await _flatRepository.Get(f => f.Id == flatModel.Id);
                if (flat == null) throw new TaskCanceledException("Flat not exists");

                flat.Code = flatModel.Code;
                flat.Rent = flatModel.Rent;
                flat.GasBill = flatModel.GasBill;
                flat.CleanerBill = flatModel.CleanerBill;

                bool request = await _flatRepository.Edit(flat);
                if (!request) throw new TaskCanceledException("Failed to edit flat");
                return request;
            }
            catch { throw; }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var flat = await _flatRepository.Get(f => f.Id == id);
                if (flat == null) throw new TaskCanceledException("Flat not exists");


                bool request = await _flatRepository.Delete(flat);
                if (!request) throw new TaskCanceledException("Failed to delete flat");
                return request;
            }
            catch { throw; }
        }


    }
}
