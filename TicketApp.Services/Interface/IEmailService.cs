using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketApp.Domain.DomainModels;

namespace TicketApp.Services.Interface
{
   
        public interface IEmailService
        {
            Task SendEmailAsync(List<EmailMessage> allMails);
        }
    
}
