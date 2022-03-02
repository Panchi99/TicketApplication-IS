using System;
using System.Collections.Generic;
using System.Text;

namespace TicketApp.Domain.DomainModels
{
        public class StripeSettings
        {
            public string SecretKey { get; set; }
            public string PublishableKey { get; set; }
        }
    
}
