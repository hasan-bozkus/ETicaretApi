using D = ETicaretApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretApi.Application.Abstractions.Services.Authentication
{
    public interface IExternalAuthentication
    {
        Task<D::Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);
        Task<D::Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime);
    }
}
