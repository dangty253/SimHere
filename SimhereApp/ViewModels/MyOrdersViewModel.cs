using System;
using SimHere.Entities;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class MyOrdersViewModel : ListViewPageViewModel<Order>
    {
        public int? Status { get; set; }
        public int OrderType { get; set; }  // mua hay ban 0 mua 1 ban
        public MyOrdersViewModel()
        {
            PreLoadData = new Command(() => {
                if (Status.HasValue)
                {
                    ApiUrl = $"api/order/me?OrderType={OrderType}&Status={Status}&Page={Page}";
                }
                else
                {
                    ApiUrl = $"api/order/me?OrderType={OrderType}&Page={Page}";
                }
            });
        }
    }
}
