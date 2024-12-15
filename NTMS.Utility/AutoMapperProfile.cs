using AutoMapper;
using NTMS.DTO;
using NTMS.Model;
using System.Globalization;

namespace NTMS.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Flat
            CreateMap<Flat, FlatDTO>().ForMember(dest => dest.Rent, opt => opt.MapFrom(origin => Convert.ToString(origin.Rent, new CultureInfo("en-US"))))
                               .ForMember(dest => dest.GasBill, opt => opt.MapFrom(origin => Convert.ToString(origin.GasBill, new CultureInfo("en-US"))))
               .ForMember(dest => dest.CleanerBill, opt => opt.MapFrom(origin => Convert.ToString(origin.CleanerBill, new CultureInfo("en-US"))));

            CreateMap<FlatDTO, Flat>().ForMember(dest => dest.Rent, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rent, new CultureInfo("en-US"))))
                                .ForMember(dest => dest.GasBill, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.GasBill, new CultureInfo("en-US"))))
                .ForMember(dest => dest.CleanerBill, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.CleanerBill, new CultureInfo("en-US"))));


            #endregion Flat

            #region Tenant
            CreateMap<Tenant, TenantDTO>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => origin.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0))
                .ForMember(dest => dest.FlatCode, opt => opt.MapFrom(origin => origin.Flat.Code));

            CreateMap<TenantDTO, Tenant>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false));

            #endregion Tenant


            #region Emeter
            CreateMap<Emeter, EmeterDTO>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0))
                .ForMember(dest => dest.FlatCode, opt => opt.MapFrom(origin => origin.Flat.Code));

            CreateMap<EmeterDTO, Emeter>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false))
                .ForMember(dest => dest.Flat, opt => opt.Ignore());

            #endregion Emeter

            #region Ereading
            CreateMap<Ereading, EreadingDTO>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => origin.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(origin => origin.EndDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EmeterNumber, opt => opt.MapFrom(origin => Convert.ToString(origin.Emeter.MeterNumber, new CultureInfo("en-US"))));

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

            #region Wmeter
            CreateMap<Wmeter, WmeterDTO>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == true ? 1 : 0))
                .ForMember(dest => dest.FlatCode, opt => opt.MapFrom(origin => origin.Flat.Code));

            CreateMap<WmeterDTO, Wmeter>().ForMember(dest => dest.IsActive, opt => opt.MapFrom(origin => origin.IsActive == 1 ? true : false))
                .ForMember(dest => dest.Flat, opt => opt.Ignore());

            #endregion Wmeter


            #region Wreading
            CreateMap<Wreading, WreadingDTO>().ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => origin.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(origin => origin.EndDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.WmeterNumber, opt => opt.MapFrom(origin => Convert.ToString(origin.Wmeter.MeterNumber, new CultureInfo("en-US"))));

            CreateMap<WreadingDTO, Wreading>().ForMember(dest => dest.Wmeter, opt => opt.Ignore())
          .ForMember(dest => dest.StartDate, opt => opt.MapFrom(origin => Convert.ToDateTime(origin.StartDate)))
          .ForMember(dest => dest.EndDate, opt => opt.MapFrom(origin => Convert.ToDateTime(origin.EndDate)));

            #endregion Wreading

            #region EBillingRule
            CreateMap<EbillingRule, EbillingRuleDTO>().ForMember(dest => dest.From1, opt => opt.MapFrom(origin => Convert.ToString(origin.From1, new CultureInfo("en-US"))))
                .ForMember(dest => dest.From2, opt => opt.MapFrom(origin => Convert.ToString(origin.From2, new CultureInfo("en-US"))))
                .ForMember(dest => dest.From3, opt => opt.MapFrom(origin => Convert.ToString(origin.From3, new CultureInfo("en-US"))))
                .ForMember(dest => dest.From4, opt => opt.MapFrom(origin => Convert.ToString(origin.From4, new CultureInfo("en-US"))))
                .ForMember(dest => dest.To1, opt => opt.MapFrom(origin => Convert.ToString(origin.To1, new CultureInfo("en-US"))))
                .ForMember(dest => dest.To2, opt => opt.MapFrom(origin => Convert.ToString(origin.To2, new CultureInfo("en-US"))))
                .ForMember(dest => dest.To3, opt => opt.MapFrom(origin => Convert.ToString(origin.To3, new CultureInfo("en-US"))))
                .ForMember(dest => dest.To4, opt => opt.MapFrom(origin => Convert.ToString(origin.To4, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Rate1, opt => opt.MapFrom(origin => Convert.ToString(origin.Rate1, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Rate2, opt => opt.MapFrom(origin => Convert.ToString(origin.Rate2, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Rate3, opt => opt.MapFrom(origin => Convert.ToString(origin.Rate3, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Rate4, opt => opt.MapFrom(origin => Convert.ToString(origin.Rate4, new CultureInfo("en-US"))))
                .ForMember(dest => dest.DemandCharge, opt => opt.MapFrom(origin => Convert.ToString(origin.DemandCharge, new CultureInfo("en-US"))))
                .ForMember(dest => dest.CommercialRate, opt => opt.MapFrom(origin => Convert.ToString(origin.CommercialRate, new CultureInfo("en-US"))))
                .ForMember(dest => dest.CommercialDc, opt => opt.MapFrom(origin => Convert.ToString(origin.CommercialDc, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(origin => Convert.ToString(origin.Vat, new CultureInfo("en-US"))));

            CreateMap<EbillingRuleDTO, EbillingRule>().ForMember(dest => dest.From1, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.From1, new CultureInfo("en-US"))))
             .ForMember(dest => dest.From2, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.From2, new CultureInfo("en-US"))))
             .ForMember(dest => dest.From3, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.From3, new CultureInfo("en-US"))))
             .ForMember(dest => dest.From4, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.From4, new CultureInfo("en-US"))))
             .ForMember(dest => dest.To1, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.To1, new CultureInfo("en-US"))))
             .ForMember(dest => dest.To2, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.To2, new CultureInfo("en-US"))))
             .ForMember(dest => dest.To3, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.To3, new CultureInfo("en-US"))))
             .ForMember(dest => dest.To4, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.To4, new CultureInfo("en-US"))))
             .ForMember(dest => dest.Rate1, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rate1, new CultureInfo("en-US"))))
             .ForMember(dest => dest.Rate2, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rate2, new CultureInfo("en-US"))))
             .ForMember(dest => dest.Rate3, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rate3, new CultureInfo("en-US"))))
             .ForMember(dest => dest.Rate4, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Rate4, new CultureInfo("en-US"))))
             .ForMember(dest => dest.DemandCharge, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.DemandCharge, new CultureInfo("en-US"))))
             .ForMember(dest => dest.CommercialRate, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.CommercialRate, new CultureInfo("en-US"))))
             .ForMember(dest => dest.CommercialDc, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.CommercialDc, new CultureInfo("en-US"))))
             .ForMember(dest => dest.Vat, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Vat, new CultureInfo("en-US"))));
            #endregion EBillingRule

            #region WBillingRule
            CreateMap<WbillingRule, WbillingRuleDTO>().ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(origin => Convert.ToString(origin.UnitPrice, new CultureInfo("en-US"))))
                .ForMember(dest => dest.ServiceCharge, opt => opt.MapFrom(origin => Convert.ToString(origin.ServiceCharge, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(origin => Convert.ToString(origin.Vat, new CultureInfo("en-US"))));

            CreateMap<WbillingRuleDTO, WbillingRule>().ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.UnitPrice, new CultureInfo("en-US"))))
                .ForMember(dest => dest.ServiceCharge, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.ServiceCharge, new CultureInfo("en-US"))))
                .ForMember(dest => dest.Vat, opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Vat, new CultureInfo("en-US"))));

            #endregion WBillingRule
        }

    }
}
