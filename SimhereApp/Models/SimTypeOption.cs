using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimhereApp.Portable.Models
{
    public class SimTypeOption : BaseViewModel
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public SimTypeOption(string name)
        {
            Name = name;
        }



    }
    public class SimTypeOptionData
    {
        public static List<SimTypeOption> Options()
        {
            var list = new List<SimTypeOption>()
            {
                new SimTypeOption("Ngày sinh"),
                new SimTypeOption("Lục quý"),
                    new SimTypeOption("Ngũ quý"),
                    new SimTypeOption("Tư quý"),
                    new SimTypeOption("Lục quý giữa"),
                    new SimTypeOption("Ngũ quý giữa"),
                    new SimTypeOption("Thần tài"),
                    new SimTypeOption("Lộc phát"),
                    new SimTypeOption("Tam hoa"),
                    new SimTypeOption("Ông địa"),
                    new SimTypeOption("Tam hoa kép"),
                    new SimTypeOption("Taxi"),
                    new SimTypeOption("Tiến lên"),
                    new SimTypeOption("Dễ nhớ"),
                    new SimTypeOption("Vip"),
                    new SimTypeOption("Tiến lên"),
                    new SimTypeOption("Lặp kép"),
                    new SimTypeOption("Tự chọn"),
                    new SimTypeOption("Trả góp"),
                    new SimTypeOption("Viettel F90"),
                    new SimTypeOption("Trả sau"),
                    new SimTypeOption("Giá rẻ"),
                    new SimTypeOption("Khuyến mãi"),
                    new SimTypeOption("Đạt cái"),
                    new SimTypeOption("Gánh"),
                    new SimTypeOption("Đầu số cổ")
            };
            return list;
        }
    }

}
