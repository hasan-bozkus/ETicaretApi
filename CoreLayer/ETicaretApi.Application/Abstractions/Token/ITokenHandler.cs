﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T = ETicaretApi.Application.Abstractions.DTOs;

namespace ETicaretApi.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        T::Token CreateAccessToken(int munite);


    }
}
