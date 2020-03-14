using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuyingViewCell : ViewCell
    {
        public static BindableProperty PostModelProperty = BindableProperty.Create(
            nameof(PostModel)
            , typeof(Post)
            , typeof(SellingViewCell)
            , null
            , BindingMode.TwoWay);
        public Post PostModel
        {
            get => (Post)GetValue(PostModelProperty);
            set => SetValue(PostModelProperty, value);
        }

        public static BindableProperty ContainerBindingContextProperty = BindableProperty.Create(
           nameof(ContainerBindingContext)
           , typeof(object)
           , typeof(SellingViewCell));
        public object ContainerBindingContext
        {
            get => (object)GetValue(ContainerBindingContextProperty);
            set => SetValue(ContainerBindingContextProperty, value);
        }
        public BuyingViewCell()
        {
            InitializeComponent();
        }
    }
}