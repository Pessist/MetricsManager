using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.DAL.Models;
using MetricsAgent.Controllers;
using MetricsAgent.DAL.Interfaces;
using MetricsAgent.DAL.Repository;
using System.Data.SQLite;




namespace MetricsAgent
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

            services.AddControllers();            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });
            });
            ConfigureSqlLiteConnection(services);
            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            using var command = new SQLiteCommand(connection);

            command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY, value INT, time INT)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(5, 100)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(2, 50)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(9, 150)";
            command.ExecuteNonQuery();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
