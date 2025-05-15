
using CyberSphere.BLL.Mapping;
using CyberSphere.BLL.Services.Implementation;
using CyberSphere.BLL.Services.Implenentation;
using CyberSphere.BLL.Services.Interface;
using CyberSphere.DAL.Database;
using CyberSphere.DAL.Entities;
using CyberSphere.DAL.Repo.Implementation;
using CyberSphere.DAL.Repo.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using CyberSphere.BLL.Helper;
using System.Text;
namespace CyberSphere.PLL   
{
    public class Program
    {
        public static async Task /*void*/ Main(string[] args)
        {


          //  logs

            //Directory.CreateDirectory("Logs"); // تأكد من وجود المجلد

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.File(
            //        path: "Logs/log-.txt",
            //        rollingInterval: RollingInterval.Day,
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
            //    )
            //    .CreateLogger();

            //var builder = WebApplication.CreateBuilder(args);

            //builder.Host.UseSerilog(); // استخدم Serilog بدلاً من الـ Logger الافتراضي



            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")),ServiceLifetime.Scoped);


            builder.Services.AddTransient<IEmailSender, EmailService>();

            builder.Services.AddScoped<IArticleRepo, ArticleRepo>();    
            builder.Services.AddScoped<IArticleService,ArticleService>();

            builder.Services.AddScoped<ILessonRepo, LessonRepo>();
            builder.Services.AddScoped<ILessonService, LessonService>();

            builder.Services.AddScoped<ICourseRepo, CourseRepo>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            builder.Services.AddScoped<ILevelRepo, LevelRepo>();
            builder.Services.AddScoped<ILevelService, LevelService>();

            builder.Services.AddScoped<IBookRepo, BookRepo>();
            builder.Services.AddScoped<IBookService, BookService>();

            builder.Services.AddScoped<IStudentRepo, StudentRepo>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddScoped<IProgressRepo, ProgressRepo>();
            builder.Services.AddScoped<IProgressService, ProgressService>();

            builder.Services.AddScoped<ICertificateRepo, CertificateRepo>();
            builder.Services.AddScoped<ICertificateService, CertificateService>();

            builder.Services.AddScoped<IPdfGeneratorService,PdfGeneratorService>();

            builder.Services.AddScoped<ISkillRepo, SkillRepo>();
            builder.Services.AddScoped<ISkillService, SkillService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            options.SignIn.RequireConfirmedEmail = true
            );

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(8));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:issuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:audienceIP"],

                    IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            }).AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

        
                options.Scope.Add("email"); 
                options.Fields.Add("email");
            }).AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:AppId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:AppSecret"];
            })
            ;
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Cyber Sphere Web API",
                    Description = " Cyber Acadmy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            }

                   );

            var app = builder.Build();
            // Seed admin user and role here
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                try
                {
                    await IdentitySeedData.SeedAdminUser(services, config);
                    Console.WriteLine($"[Program.cs] Admin Email from config: {config["AdminUser:Email"]}");
                }
                catch (Exception ex)
                {
                    // Log exception or handle errors here
                    Console.WriteLine($"Error seeding admin user: {ex.Message}");
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
                app.UseSwaggerUI();
         
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                // مسار نسبي، هيحفظ الملف بجانب الملفات المنشورة (داخل wwwroot مثلاً)
                File.WriteAllText("wwwroot/error_log.txt", ex.ToString());
            }
        }
    }
}
