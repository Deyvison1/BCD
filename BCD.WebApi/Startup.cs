using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Repository.Data;
using BCD.Repository.EntitiesRepository.ContaRepository;
using BCD.Repository.EntitiesRepository.EnderecoRepository;
using BCD.Repository.EntitiesRepository.EnderecosPessoasRepository;
using BCD.Repository.EntitiesRepository.HistoricoRepository;
using BCD.Repository.EntitiesRepository.PessoaRepository;
using BCD.WebApi.Services.ContaServices;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.HistoricoServices;
using BCD.WebApi.Services.PessoaServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BCD.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PessoaService>();
            services.AddScoped<ContaService>();
            services.AddScoped<HistoricoService>();
            services.AddScoped<EnderecoService>();
            services.AddScoped<IEnderecosPessoasRepository, EnderecosPessoasRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IHistoricoRepository, HistoricoRepository>();
            services.AddScoped<IContaRepository, ContaRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddDbContext<BCDContext>(x => x.UseMySql(Configuration.GetConnectionString("Connection")));
            services.AddMvc().AddJsonOptions(
                x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());            
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
