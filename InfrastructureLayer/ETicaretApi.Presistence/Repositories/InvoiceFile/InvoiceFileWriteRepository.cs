﻿using ETicaretApi.Application.Repostories;
using ETicaretApi.Domain.Entities;
using ETicaretApi.Presistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Presistence.Repositories
{
    public class InvoiceFileWriteRepository : WriteRepository<InvoceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(ETicaretApiDbContext context) : base(context)
        {
        }
    }
}
