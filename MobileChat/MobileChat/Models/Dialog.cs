using System;
using System.Collections.Generic;
using System.Text;

namespace MobileChat.Models
{
    [Serializable]
    public class Dialog
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string UserIDs { get; set; }
        public string Name { get; set; }
    }
}
