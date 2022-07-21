using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Market.Presentation.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);

builder.ConfigureServices().Build()
        .ConfigurePipelines().Run();
public partial class Program { }
