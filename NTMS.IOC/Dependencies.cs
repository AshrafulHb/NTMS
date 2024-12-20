﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTMS.BLL.Services;
using NTMS.BLL.Services.Abstract;
using NTMS.DAL.DBContext;
using NTMS.DAL.Repository;
using NTMS.DAL.Repository.Abstract;
using NTMS.Utility;

namespace NTMS.IOC
{
    public static class Dependencies
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NtmsContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TmsConnection"));
            });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IEmeterService, EmeterService>();
            services.AddScoped<IEreadingService, EreadingService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IWmeterService, WmeterService>();
            services.AddScoped<IWreadingService, WreadingService>();
            services.AddScoped<IEruleService, EruleService>();
            services.AddScoped<IWruleService, WruleService>();

        }
    }
}
