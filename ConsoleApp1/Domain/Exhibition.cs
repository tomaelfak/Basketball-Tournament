using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Domain
{
    public class Exhibition
    {
        public string? Date { get; set; }
        public required string Opponent { get; set; }

        public required string Result { get; set; }
    }
}