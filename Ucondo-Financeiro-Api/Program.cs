using FluentValidation.AspNetCore;
using GFL.Infra;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Ucondo_Financeiro_Api;

Log.Logger = AppSerilogConfiguration.Configure();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddFluentValidation(options =>
                {
                    options.ImplicitlyValidateChildProperties = true;
                    options.ImplicitlyValidateRootCollectionElements = true;
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Ucondo-Financeiro-Api",
            Version = "v1",
            Description = "Serviço responsável pelas operações financeiras",
            Contact = new OpenApiContact
            {
                Name = "UCondo",
            }
        });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
DependencyInjectionManager.InjectAppDependecies(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ucondo-Financeiro-Api");
});

app.UseHttpsRedirection();

app.UseCors(x =>
{
    x.AllowAnyOrigin();
    x.AllowAnyHeader();
    x.WithMethods("POST", "PUT", "GET", "DELETE");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
