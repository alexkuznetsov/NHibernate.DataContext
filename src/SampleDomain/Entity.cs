using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDomain
{
    public abstract class Entity
    {
        public long Id { get; set; }
        public string CorrType { get; set; }

        public DateTime DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
