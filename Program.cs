using SLAPScheduling.Infrastructure.Repository.Warehouses;

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

            builder.Services.AddMediatR(config =>
                config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            builder.Services.AddScoped<ISchedulingRepository, SchedulingRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepositpory>();
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

}
