using Microsoft.Extensions.DependencyInjection;
using SLAP.AggregateModels.DeviceAggregate;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.TechnicianAggregate;
using SLAP.AggregateModels.WareHouseMaterialAggregate;
using SLAP.AggregateModels.WorkAggregate;
using SLAP.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWorkObjectInputRepository, WorkObjectInputRepository>();
builder.Services.AddScoped<ITechnicianObjectInputRepository, TechnicianObjectInputRepository>();
builder.Services.AddScoped<IDeviceObjectInputRepository, DeviceObjectInputRepository>();
builder.Services.AddScoped<IMaterialObjectInputRepository, MaterialObjectInputRepository>();
builder.Services.AddScoped<IWareHouseMaterialObjectInputRepository, WareHouseMaterialObjectInputRepository>();
builder.Services.AddScoped<IObjectInputRepository, ObjectInputRepository>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<WorkObjectInput>();
});
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
