using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomain.Model
{
    public class Message
    {
        public string Text { get; set; }
        public long UserId { get; set; }
        public bool IsReaded { get; set; }
    }
}
