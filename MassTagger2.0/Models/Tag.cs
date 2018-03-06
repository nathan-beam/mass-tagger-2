using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MassTagger2.Models
{
    public class Tag
    {
        [JsonIgnoreAttribute]
        public string Name { get; set; }

        public string color { get; set; }

        public bool ignored { get; set;}

        public string link { get; set; }

        public string text { get; set; }
    }
}