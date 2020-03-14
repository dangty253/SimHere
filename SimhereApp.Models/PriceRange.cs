using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimhereApp.Models
{
    public class PriceRange
    {
        public decimal? From { get; set; }
        public decimal? To { get; set; }

        public string Display { get; set; }

        public PriceRange(string display, decimal? from, decimal? to)
        {
            Display = display;
            From = from;
            To = to;
        }


        public static List<PriceRange> GetList()
        {
            List<PriceRange> list = new List<PriceRange>();
            list.Add(new PriceRange("Dưới 500 ngàn", 0, 500000));
            list.Add(new PriceRange("500 ngàn - 1 triệu", 500000, 1000000));
            list.Add(new PriceRange("1 triệu - 3 triệu", 1000000, 3000000));
            list.Add(new PriceRange("3 triệu - 5 triệu", 3000000, 5000000));
            list.Add(new PriceRange("5 triệu - 10 triệu", 5000000, 10000000));
            list.Add(new PriceRange("10 triệu - 20 triệu", 10000000, 20000000));
            list.Add(new PriceRange("", 0, 500000));
            list.Add(new PriceRange("", 0, 500000));
            list.Add(new PriceRange("", 0, 500000));
            return list;
        }
    }
}
