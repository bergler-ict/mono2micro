using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SalesService.Data.Context
{
    public class SalesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SalesDbContext>
    {
        public SalesDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SalesDbContext>();

            builder.UseSqlServer("[your_connectstring]");

            return new SalesDbContext(builder.Options);
        }
    }
}
