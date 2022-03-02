using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketApp.Domain.DomainModels;
using TicketApp.Services.Interface;
using static TicketApp.Repository.Interface.IRepository;

namespace TicketApp.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;


        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }

        public async Task DoWork()
        {

            await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }
}
