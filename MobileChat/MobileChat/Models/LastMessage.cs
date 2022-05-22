using System;
using System.Collections.Generic;
using System.Text;

namespace MobileChat.Models
{
    public class LastMessage
    {
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeSent { get; set; }
    }
}
