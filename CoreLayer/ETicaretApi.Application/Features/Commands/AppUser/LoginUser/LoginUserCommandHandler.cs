using I = ETicaretApi.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretApi.Application.Exceptions;

namespace ETicaretApi.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<I::AppUser> _userManager;
        readonly SignInManager<I::AppUser> _signInManager;

        public LoginUserCommandHandler(UserManager<I::AppUser> userManager, SignInManager<I.AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            I::AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Kullanıcı veya Şifre hatalı...");

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded) //Authentication başarılı
            {
                //... Yetkileri belirlememiz gerekiyor...
            }

            return new();
        }
    }
}
