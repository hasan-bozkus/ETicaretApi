using ETicaretApi.Domain.Entities.Common;
using ETicaretApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Domain.Entities
{
	public class Basket : BaseEntity
	{
        public string UserId { get; set; }
        public AppUser User { get; set; }

		public ICollection<BasketItem> BasketItems { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
