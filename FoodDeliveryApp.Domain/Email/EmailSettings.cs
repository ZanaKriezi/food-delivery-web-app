﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Domain.Email
{
    public class EmailSettings
    {
        public string SmtpServer {  get; set; }
        public string SmtpUserName{  get; set; }
        public string SmtpPassword{  get; set; }
        public int SmtpServerPort{  get; set; }
        public bool Enablessl {  get; set; }
        public string  EmailDisplayName{  get; set; }
        public string  SenderName {  get; set; }

        public EmailSettings() { }
        public EmailSettings(string smtpServer, string smptUserName, string smtpPassword, int smptServerPort) {
            this.SmtpServer = smtpServer;
            this.SmtpUserName = smptUserName;
            this.SmtpPassword = smtpPassword;
            this.SmtpServerPort = smptServerPort;

        }

    }
}
