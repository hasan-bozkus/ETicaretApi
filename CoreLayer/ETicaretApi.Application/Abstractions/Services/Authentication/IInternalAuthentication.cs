using D = ETicaretApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<D::Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime);
    }
}
