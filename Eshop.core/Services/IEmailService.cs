﻿using Eshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
