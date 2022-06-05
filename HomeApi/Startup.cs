using FluentValidation.AspNetCore;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using HomeApi.Data;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HomeApi
{
    public class Startup
    {
        /// <summary>
        /// Загрузка конфигурации из файла Json
        /// </summary>
        private IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("HomeOptions.json")
            .Build();

        //public Startup(IConfiguration configuration)
        //{
        //    //Configuration = configuration;
        //}


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Подключаем автомаппинг
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            services.AddAutoMapper(assembly);

            // регистрация сервиса репозитория для взаимодействия с базой данных
            services.AddSingleton<IDeviceRepository, DeviceRepository>();
            services.AddSingleton<IRoomRepository, RoomRepository>();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

            // Подключаем валидацию
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

            // Добавляем новый сервис
            services.Configure<HomeOptions>(Configuration);

            // Загружаем только адресс (вложенный Json-объект))
            services.Configure<Address>(Configuration.GetSection("Address"));

            // Нам не нужны представления, но в MVC бы здесь стояло AddControllersWithViews()
            services.AddControllers();
            // поддерживает автоматическую генерацию документации WebApi с использованием Swagger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
