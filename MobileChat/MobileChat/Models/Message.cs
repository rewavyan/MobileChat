using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileChat.Models
{
    [Serializable]
    public class Message
    {
        public int ID { get; set; }
        public string DialogID { get; set; }
        public string UserID { get; set; }
        public string Value { get; set; }
        public DateTime DateTimeSent { get; set; }
        public string UserName { get; set; }
        public bool IsYour { get; set; }
        public Color BoxColor { get; set; }
        public Thickness BoxMargin { get; set; }
        public Thickness MessageMargin { get; set; }
        public Thickness DateTimeMargin { get; set; }
        
    }
}
