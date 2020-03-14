using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimhereApp.Portable.Models
{
    public class SimDetailModel
    {
        public string Id { get; set; }

        public string OwnerId { get; set; }
        public Users Owner { get; set; }

        public string SimNumber { get; set; }
        public string DisplayNumber { get; set; }
        public Carrier Carrier { get; set; }

        public SubcribeType SubcribeType { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }
        public string TimeAgo => DateTimeHelper.TimeAgo(CreatedOn);
        public int Status { get; set; }
        public string StatusFormat { get; set; }
    }
}
