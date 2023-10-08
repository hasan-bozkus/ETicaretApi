using ETicaretApi.Presistence.Contexts;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ETicaretApi.Presistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETicaretApiDbContext>
    {
        public ETicaretApiDbContext CreateDbContext(string[] args)
        {
           

            DbContextOptionsBuilder<ETicaretApiDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);
            return new(dbContextOptionsBuilder.Options);
        }
    }
}
