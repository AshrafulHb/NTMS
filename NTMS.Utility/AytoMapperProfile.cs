using AutoMapper;
using NTMS.DTO;
using NTMS.Model;
using System.Globalization;

namespace NTMS.Utility
{
    public class AytoMapperProfile:Profile
    {
        public AytoMapperProfile()
        {
            #region Flat
            CreateMap<Flat, FlatDTO>().ForMember(dest => dest.Rent, opt => opt.MapFrom(origin => Convert.ToString(origin.Rent, new CultureInfo("en-US"))));
            CreateMap<FlatDTO, Flat>().ForMember(dest => dest.Rent, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rent, new CultureInfo("en-US"))));

            #endregion Flat

            #region Tenant
            CreateMap<Tenant, TenantDTO>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => origin.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0))
                .ForMember(dest => dest.FlatDescription, opt=>opt.MapFrom(origin=>origin.Flat.Code));

            CreateMap<TenantDTO, Tenant>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false));

            #endregion Tenant


            #region Emeter
            CreateMap<Emeter, EmeterDTO>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0))
                .ForMember(dest => dest.FlatDescription, opt => opt.MapFrom(origin => origin.Flat.Code));

            CreateMap<EmeterDTO, Emeter>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false))
                .ForMember(dest => dest.Flat, opt => opt.Ignore());

            #endregion Emeter

            #region Ereading
            CreateMap<Ereading, EreadingDTO>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => origin.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(origin => origin.EndDate.ToString("dd/MM/yyyy")))
                .ForMember(dest=>dest.EmeterNumber,opt=>opt.MapFrom(origin=>Convert.ToString(origin.Emeter.MeterNumber, new CultureInfo("en-US"))));

            CreateMap<EreadingDTO, Ereading>().ForMember(dest => dest.Emeter, opt => opt.Ignore())
          .ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => Convert.ToDateTime(origin.StartDate)))
          .ForMember(dest => dest.EndDate, opt => opt.MapFrom(origin => Convert.ToDateTime(origin.EndDate)));

            #endregion Ereading

            #region Report
            CreateMap<Report, ReportDTO>()
                .ForAllMembers(o => o.Condition((src, dest, ValueTask) => ValueTask != null));
            CreateMap<ReportDTO, Report>()
                .ForAllMembers(o => o.Condition((src, dest, ValueTask) => ValueTask != null));
            #endregion Report
        }

    }
}
