using SimHere.Entities;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class SimFollowViewModel : ListViewPageViewModel<Sim>
    {
        public SimFollowViewModel()
        {
            PreLoadData = new Command(() => ApiUrl = $"api/sim/myfollowsims?Page={Page}");
        }
    }
}
