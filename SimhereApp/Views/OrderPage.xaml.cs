using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class OrderPage : ContentPage
    {
        private readonly OrderViewModel viewModel;
        public OrderPage(string simId, decimal? fixedPrice)
        {
            InitializeComponent();
            BindingContext = viewModel = new OrderViewModel(simId, fixedPrice);
        }
        public OrderPage(string orderId)
        {
            InitializeComponent();
            BindingContext = viewModel = new OrderViewModel(orderId);
        }       
    }
}
