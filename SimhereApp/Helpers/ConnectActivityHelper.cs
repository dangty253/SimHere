using Xamarin.Essentials;

namespace SimhereApp.Portable.Helpers
{
    public class ConnectActivityHelper
    {
        public ConnectActivityHelper()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

       void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            if (access == NetworkAccess.None)
            {
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Mất kết nối Internet. Vui lòng thử lại", "Đóng");
            }
        }
    }
}
