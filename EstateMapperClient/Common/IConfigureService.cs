﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstateMapperLibrary.Models;

namespace EstateMapperClient.Common
{
    public interface IConfigureService
    {
        void Configure(LoginResponse response);
    }
}
