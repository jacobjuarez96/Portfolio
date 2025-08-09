using BlazorApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(ContactFormModel model);
    }
}