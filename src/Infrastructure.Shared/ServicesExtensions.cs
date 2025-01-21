using Application.Contracts.Services;
using Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Infrastructure.Shared
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILoggerService, LoggerService>();

            #region Serilog Settings

            // string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // string connectionString = configuration.GetConnectionString("DefaultConnection");
            // const string tableName = "LOGS";

            ColumnOptions columnOptions = new()
            {
                AdditionalColumns = new List<SqlColumn>() {
                    new() { ColumnName = "Action" },
                    new() { ColumnName = "CompanyId", DataType = System.Data.SqlDbType.Int },
                    new() { ColumnName = "UserId" }
                }
            };

            // Log.Logger = new LoggerConfiguration()
            //     .MinimumLevel.Information()
            //     .WriteTo.MSSqlServer(
            //         connectionString: connectionString,
            //         sinkOptions: new()
            //         {
            //             AutoCreateSqlTable = false,
            //             TableName = tableName
            //         },
            //         columnOptions: columnOptions
            //     ).CreateLogger();

            #endregion

            return services;
        }
    }
}