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
    public class FileReadRepository : ReadRepository<F::File>, IFileReadRepository
    {
        public FileReadRepository(ETicaretApiDbContext context) : base(context)
        {
        }
    }
}
