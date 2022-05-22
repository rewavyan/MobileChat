using System;
using System.Collections.Generic;
using System.Text;

namespace MobileChat.Models
{
    [Serializable]
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string DialogIDs { get; set; }
    }
}
