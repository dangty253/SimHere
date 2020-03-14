using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimhereApp.Models
{
    public class SimCategory
    {
        public string Name { get; set; }
        public SimCategory()
        {

        }
        public SimCategory(string name)
        {
            Name = name;
        }

        public bool IsSelected { get; set; }

        public static List<SimCategory> GetPriceCategory()
        {
            List<SimCategory> simCategories = new List<SimCategory>()
            {
                new SimCategory("Dưới 500 ngàn"),
                new SimCategory("500 ngàn - 1 triệu"),
                new SimCategory("1 triệu - 3 triệu"),
                new SimCategory("5 triệu - 10 triệu"),
                new SimCategory("50 triệu - 100 triệu"),
                new SimCategory("100 triệu - 200 triệu"),
                new SimCategory("Trên 200 triệu"),
            };
            return simCategories;
        }

        public static List<SimCategory> GetNumberTypeCategory()
        {
            List<SimCategory> simCategories = new List<SimCategory>()
            {
                new SimCategory("Lục quý"),
                    new SimCategory("Ngũ quý"),
                    new SimCategory("Tư quý"),
                    new SimCategory("Lục quý giữa"),
                    new SimCategory("Ngũ quý giữa"),
                    new SimCategory("Thần tài"),
                    new SimCategory("Lộc phát"),
                    new SimCategory("Tam hoa"),
                    new SimCategory("Ông địa"),
                    new SimCategory("Tam hoa kép"),
                    new SimCategory("Taxi"),
                    new SimCategory("Tiến lên"),
                    new SimCategory("Dễ nhớ"),
                    new SimCategory("Vip"),
                    new SimCategory("Tiến lên"),
                    new SimCategory("Lặp kép"),
                    new SimCategory("Tự chọn"),
                    new SimCategory("Trả góp"),
                    new SimCategory("Viettel F90"),
                    new SimCategory("Trả sau"),
                    new SimCategory("Giá rẻ"),
                    new SimCategory("Khuyến mãi"),
                    new SimCategory("Đạt cái"),
                    new SimCategory("Gánh"),
                    new SimCategory("Đầu số cổ")
            };
            return simCategories;
        }

        public static List<SimCategory> GetNumberHeadCategory()
        {
            List<SimCategory> simCategories = new List<SimCategory>()
            {
                new SimCategory("090 - Mobifone"),
                    new SimCategory("091 - Vinaphone"),
                    new SimCategory("092 - Vietnamobile"),
                    new SimCategory("093 - Mobifone"),
                    new SimCategory("094 - Vinaphone"),
                    new SimCategory("096 - Viettel"),
                    new SimCategory("097 - Viettel"),
                    new SimCategory("098 - Viettel"),
                    new SimCategory("099 - Gmobile")
            };
            return simCategories;
        }
    }

}
