using System;
using System.Collections.Generic;

namespace SimhereApp.Portable.StaticOptions
{
    public class BaseOption
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }


    public static class OptionCenter
    {
        public static List<BaseOption> Get(string type)
        {
            if (type == nameof(NumberTypeOptions))
            {
                return NumberTypeOptions.GetOptions();
            }
            if (type == nameof(BuySellOptions))
            {
                return BuySellOptions.GetOptions();
            }
            return null;
        }
    }
    public static class BuySellOptions
    {
        public static List<BaseOption> GetOptions()
        {
            return new List<BaseOption>()
            {
                new BaseOption{ Key = 1, Value = "Cần mua"},
                new BaseOption{ Key = 2, Value = "Cần bán"}
            };
        }
    }
    

    public static class NumberTypeOptions
    {
        public static List<BaseOption> GetOptions()
        {
            return new List<BaseOption>()
            {
                new BaseOption{ Key = 1,  Value = "Sim thường" },
                new BaseOption{ Key = 2,  Value = "Lục quý"}      ,
                new BaseOption{ Key = 3,  Value = "Ngũ quý"}      ,
                new BaseOption{ Key = 4,  Value = "Tứ quý"}      ,
                new BaseOption{ Key = 5,  Value = "Lục quý giữa" }      ,
                new BaseOption{ Key = 6,  Value = "Ngũ quý giữa"}      ,
                new BaseOption{ Key = 7,  Value = "Thần tài" }      ,
                new BaseOption{ Key = 8,  Value = "Lộc phát"}      ,
                new BaseOption{ Key = 9,  Value = "Tam hoa"}     ,
                new BaseOption{ Key = 10, Value = "Ông địa"}     ,
                new BaseOption{ Key = 11, Value = "Tam hoa kép" }     ,
                new BaseOption{ Key = 12, Value = "Taxi"}     ,
                new BaseOption{ Key = 13, Value = "Tiến lên"}     ,
                new BaseOption{ Key = 14, Value = "Dễ nhớ" }     ,
                new BaseOption{ Key = 15, Value = "Vip"}     ,
                new BaseOption{ Key = 16, Value = "Tiến lên" }     ,
                new BaseOption{ Key = 17, Value = "Lặp kép"}     ,
                new BaseOption{ Key = 18, Value = "Tự chọn"}     ,
                new BaseOption{ Key = 19, Value = "Trả góp"}     ,
                new BaseOption{ Key = 20, Value = "Viettel F9"}     ,
                new BaseOption{ Key = 21, Value = "Trả sau"}     ,
                new BaseOption{ Key = 22, Value = "Giá rẻ"}     ,
                new BaseOption{ Key = 23, Value = "Khuyến mãi"}     ,
                new BaseOption{ Key = 24, Value = "Đạt cái"}     ,
                new BaseOption{ Key = 25, Value = "Gánh"}     ,
                new BaseOption{ Key = 26, Value = "Đầu số cổ"}     ,
                new BaseOption{ Key = 27, Value = "Ngày sinh"}     ,
            };
        }
    }
}
