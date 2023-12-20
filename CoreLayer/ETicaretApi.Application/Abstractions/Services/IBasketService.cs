﻿using ETicaretApi.Application.ViewModels.Baskets;
using ETicaretApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Application.Abstractions.Services
{
	public interface IBasketService
	{
		public Task<List<BasketItem>> GetBasketItemAsync();
		public Task AddItemToBasketAsync(VM_Create_BasketItem basketItem);
		public Task UpdateQuantityAsync(VM_Update_BasketItem basketItem);
		public Task RemoveBasketItemAsync(string basketItemId);
	}
}