using ETicaretApi.Application.Repostories;
using ETicaretApi.Presistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = ETicaretApi.Domain.Entities;

namespace ETicaretApi.Presistence.Repositories
{
    public class FileWriteRepository : WriteRepository<F::File>, IFileWriteRepository
    {
        public FileWriteRepository(ETicaretApiDbContext context) : base(context)
        {
        }
    }
}
