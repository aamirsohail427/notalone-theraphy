using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telemedicine.Filters;
using Telemedicine.Models.Models;
using Telemedicine.Services.AutoMapper;
using Telemedicine.Services.Interfaces;
using Telemedicine.Services.Services;
using Telemedicine.Utilities;

namespace Telemedicine.App
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
            var mvcBuilder = services.AddControllersWithViews();
            mvcBuilder.AddRazorRuntimeCompilation();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<TelemedicineAppContext>(ServiceLifetime.Transient);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.AccessDeniedPath = "/AccessDenied/Index";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoleTypeStrings.Admin, policy => policy.RequireRole(UserRoleTypeStrings.Admin));
                options.AddPolicy(UserRoleTypeStrings.Therapist, policy => policy.RequireRole(UserRoleTypeStrings.Therapist));
                options.AddPolicy(UserRoleTypeStrings.Client, policy => policy.RequireRole(UserRoleTypeStrings.Client));
            });

            services.AddMvc(config =>
            {
                config.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddScoped<ISessionManager, SessionManager>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ISpecialtyLookupService, SpecialtyLookupService>();
            services.AddScoped<IDoctorProfileService, DoctorProfileService>();
            services.AddScoped<IDoctorAwardService, DoctorAwardService>();
            services.AddScoped<IDoctorEducationService, DoctorEducationService>();
            services.AddScoped<IDoctorEducationService, DoctorEducationService>();
            services.AddScoped<IDoctorExperienceService, DoctorExperienceService>();
            services.AddScoped<IPatientAttachmentService, PatientAttachmentService>();
            services.AddScoped<ISOAPNoteService, SOAPNoteService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IUserStateService, UserStateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
