using System;
using System.Collections.Generic;
using System.Linq;
using SimhereApp.Portable.ViewModels;

namespace SimhereApp.Portable.Models
{
    public class SimDetailImageBackground : BaseViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public SimDetailImageBackground(int id, string url)
        {
            Id = id;
            Url = url;
        }


        public static List<SimDetailImageBackground> GetImages()
        {
            return new List<SimDetailImageBackground>()
            {
                new SimDetailImageBackground(0,"sim1.jpg"),
                new SimDetailImageBackground(1,"https://media.istockphoto.com/photos/cards-picture-id914759318?k=6&m=914759318&s=612x612&w=0&h=IZ9yix0HfU5-uVHdoAdONmWcHYuFw8358etTgDpV33I="),
                new SimDetailImageBackground(3,"https://media.istockphoto.com/photos/cards-picture-id157191960?k=6&m=157191960&s=612x612&w=0&h=kNuknMKsGNzy-yhNmyDGg_kTxGD5JPZiUM83sjsIu_8=")
            };
        }

        public static SimDetailImageBackground GetById(int Id)
        {
            if (GetImages().Any(x => x.Id == Id))
            {
                return GetImages().SingleOrDefault(x => x.Id == Id);
            }
            else
            {
                return GetImages()[0];
            }
        }
    }
}
