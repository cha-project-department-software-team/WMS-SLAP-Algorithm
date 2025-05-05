using SLAPScheduling.Domain.InterfaceRepositories.IInventoryIssues;
using SLAPScheduling.Domain.InterfaceRepositories.IMaterial;
using SLAPScheduling.Infrastructure.Repository.InventoryIssues;

namespace SLAPScheduling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add the connection EmployeeType to the container
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add the DbContext to the container with the connection EmployeeType
            builder.Services.AddDbContext<SLAPDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SLAPScheduling")));


            // Register the AutoMapper services with the assembly
            builder.Services.AddAutoMapper(typeof(ModelToViewModelProfile).Assembly);
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            builder.Services.AddScoped<IReceiptSchedulingRepository, ReceiptSchedulingRepository>();
            builder.Services.AddScoped<IIssueSchedulingRepository, IssueSchedulingRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddScoped<IReceiptLotRepository, ReceiptLotRepository>();
            builder.Services.AddScoped<IIssueLotRepository, IssueLotRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IMaterialPropertyRepository, MaterialPropertyRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

}
