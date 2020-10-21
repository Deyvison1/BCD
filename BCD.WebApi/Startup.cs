using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCD.Domain.Entities.Identity;
using BCD.Repository.Data;
using BCD.Repository.EntitiesRepository.ContaCadastradaRepository;
using BCD.Repository.EntitiesRepository.ContaRepository;
using BCD.Repository.EntitiesRepository.EnderecoRepository;
using BCD.Repository.EntitiesRepository.HistoricoRepository;
using BCD.Repository.EntitiesRepository.HistoricosContasRepository;
using BCD.Repository.EntitiesRepository.PapeisUsuariosRepository;
using BCD.Repository.EntitiesRepository.PessoaRepository;
using BCD.WebApi.Services.ContaCadastradaServices;
using BCD.WebApi.Services.ContaServices;
using BCD.WebApi.Services.ContaServices.SolicitarContaServices;
using BCD.WebApi.Services.EnderecoServices;
using BCD.WebApi.Services.GenericServices;
using BCD.WebApi.Services.HistoricoServices;
using BCD.WebApi.Services.PessoaServices;
using BCD.WebApi.Services.UsuarioServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            services.AddScoped<SolicitarContaService>();
            services.AddScoped<EnderecoService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<ContaCadastradaService>();
            services.AddScoped<GenericService>();
            services.AddScoped<IRepositoryGeneric, RepositoryGeneric>();
            services.AddScoped<IHistoricosContasRepository, HistoricosContasRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IHistoricoRepository, HistoricoRepository>();
            services.AddScoped<IContaRepository, ContaRepository>();
            services.AddScoped<IContaCadastradaRepository, ContaCadastradaRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddDbContext<BCDContext>(x => x.UseSqlite(Configuration.GetConnectionString("Connection")));
            
            IdentityBuilder builder = services.AddIdentityCore<Usuario>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Papel), builder.Services);

            builder.AddEntityFrameworkStores<BCDContext>();
            builder.AddRoleValidator<RoleValidator<Papel>>();
            builder.AddRoleManager<RoleManager<Papel>>();
            builder.AddSignInManager<SignInManager<Usuario>>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                        (Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    }; 
                });

            services.AddMvc(
                opt => {
                    var politicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    opt.Filters.Add(new AuthorizeFilter(politicy));   
                }
            ).AddJsonOptions(
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
            
            app.UseAuthentication();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());            
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
