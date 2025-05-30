using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryApp.Domain;
using FoodDeliveryApp.Domain.Email;

namespace FoodDeliveryApp.Service.Interface
{
    public interface IEmailService
    {
        Task SendEmailMessage(EmailMessage message);
    }
}
