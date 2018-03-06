using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MassTagger2.Util.Enum;

namespace MassTagger2.Models.ViewModels
{
    public class SubredditViewModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public TagColor TagColor { get; set; }

        public bool Ignored { get; set; }
    }
}