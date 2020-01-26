using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class MessageViewModel
    {
        public string From { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
