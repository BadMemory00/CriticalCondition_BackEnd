using CriticalConditionBackend.CriticalConditionExceptions;
using CriticalConditionBackend.Models;
using CriticalConditionBackend.Services;
using CriticalConditionBackend.Utillities;
using CriticalConditionBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<CriticalConditionDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CriticalConditionDB")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = TokenUtilities.GetTokenValidationParameters(builder.Configuration);

                });

builder.Services.AddScoped<ISuperUserServices, SuperUserServices>();
builder.Services.AddScoped<ISubUserServices, SubUserServices>();

builder.Services.AddControllers(options =>
    options.Filters.Add(new HttpResponseExceptionFilter()));
        
//--------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
