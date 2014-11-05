using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrebViewer.Models
{
    public class Grid
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public IEnumerable<Header> rows { get; set; }
    }
}