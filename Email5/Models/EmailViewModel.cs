using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Email5.Models
{
    public class EmailViewModel
    {
        public string ToAddress { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}