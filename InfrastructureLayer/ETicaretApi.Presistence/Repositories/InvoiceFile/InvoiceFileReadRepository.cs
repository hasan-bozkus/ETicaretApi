using ETicaretApi.Application.Repostories;
using ETicaretApi.Domain.Entities;
using ETicaretApi.Presistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Presistence.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<InvoceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(ETicaretApiDbContext context) : base(context)
        {
        }
    }
}
