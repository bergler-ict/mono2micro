using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesService.Data.Context;

namespace SalesService.Data.Test
{
    public class SalesServiceDataBase
    {
      private IConfigurationRoot _configuration;

      // represents database's configuration
      protected DbContextOptions<SalesDbContext> _options;

      public SalesServiceDataBase()
      {
          var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");

          _configuration = builder.Build();
          _options = new DbContextOptionsBuilder<SalesDbContext>()
              .UseInMemoryDatabase("InMemoryDbForTesting")
              //.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
              .Options;
      }
    }
}