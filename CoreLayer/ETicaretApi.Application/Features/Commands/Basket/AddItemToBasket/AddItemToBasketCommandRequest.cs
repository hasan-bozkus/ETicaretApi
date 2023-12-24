using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Application.Features.Commands.Basket.AddItemToBasket
{
    public class AddItemToBasketCommandRequest : IRequest<AddItemToBasketCommandResponse>
    {
        public string ProudctId { get; set; }
        public int Quantity { get; set; }
    }
}
