using SimHere.Entities;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views.Cells;
using Xamarin.Forms;

namespace SimhereApp.Portable.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;
        DataTemplate breakTimeDataTemlate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
            this.breakTimeDataTemlate = new DataTemplate(typeof(BreakTimeViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessage;
            if (messageVm == null)
                return null;
            if (messageVm.SenderId == null)
            {
                return breakTimeDataTemlate;
            }
            else
            if (messageVm.SenderId == UserLogged.Id)
            {
                return outgoingDataTemplate;
            }
            else
            {
                return incomingDataTemplate;
            }
        }

    }
}