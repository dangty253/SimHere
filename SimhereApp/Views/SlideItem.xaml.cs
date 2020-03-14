using SimHere.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideItem : ContentView
    {
        #region Placeholder
        public static readonly BindableProperty SimProperty =
            BindableProperty.Create(nameof(Sim),
                typeof(Sim),
                typeof(SlideItem),
                null,
                BindingMode.TwoWay, propertyChanged: OnSimChanged);
        public Sim Sim { get => (Sim)GetValue(SimProperty); set => SetValue(SimProperty, value); }
        #endregion

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }
        public SlideItem()
        {
            InitializeComponent();
            LblNumberDisplay.SetBinding(Label.TextProperty, new Binding("Sim.DisplayNumber")
            {
                Source = this
            });
            LblPrice.SetBinding(Label.TextProperty, new Binding("Sim.Price")
            {
                Source = this,
                StringFormat = "{0:0,0 đ}"
            });
            ImageCarrierLogo.SetBinding(Image.SourceProperty, new Binding("ImageSource")
            {
                Source = this
            });
        }


        static void OnSimChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable != null)
            {
                SlideItem slideItem = (SlideItem)bindable;
                short CarrierId = slideItem.Sim.CarrierId;
                string logo = CarrierData.Get().SingleOrDefault(x => x.Id == CarrierId).Logo;
                slideItem.ImageSource = logo.Replace("_icon", "");


            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SimDetail(Sim.Id));
        }
    }
}