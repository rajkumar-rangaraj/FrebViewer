using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrebViewer.Models
{
    public class Header
    {
        public string FileName { get; set; }
        public string URL { get; set; }
        public string Verb { get; set; }
        public string AppPoolName { get; set; }
        public int StatusCode { get; set; }
        public int TimeTaken { get; set; }
    }
}