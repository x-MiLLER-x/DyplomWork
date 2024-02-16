using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyplomWork_2._0_WPF_
{
    public class Measurement
    {
        public int Value { get; set; }
        public string Unit { get; set; }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}
