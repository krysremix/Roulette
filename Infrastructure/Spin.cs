using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class Spin
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Colour { get; set; }
        public string Time { get; set; }
        public DateTime TimeValue => !string.IsNullOrEmpty(Time) ? DateTime.Parse(Time) : new DateTime();
    }
}
