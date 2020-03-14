using System.Windows.Input;
using SimHere.Entities;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views.Cells;
using Xamarin.Forms;

namespace SimhereApp.Portable.Helpers
{
    class PostTemplateSelector : DataTemplateSelector
    {
        DataTemplate BuyingDataTemPlate;
        DataTemplate SellingDataTemPlate;

        public PostTemplateSelector()
        {
            this.BuyingDataTemPlate = new DataTemplate(typeof(BuyingViewCell));
            this.SellingDataTemPlate = new DataTemplate(typeof(SellingViewCell));
        }
        public Post PostModel { get; set; }
        public object ContainerBindingObject { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            PostModel = item as Post;
            ContainerBindingObject = container.BindingContext;
            if (PostModel == null)
                return null;
            if (PostModel.Type == 1)
            {
                BuyingDataTemPlate.SetBinding(BuyingViewCell.PostModelProperty, new Binding(nameof(PostModel)) { Source = this });
                BuyingDataTemPlate.SetBinding(BuyingViewCell.ContainerBindingContextProperty, new Binding(nameof(ContainerBindingObject)) { Source = this });
                return BuyingDataTemPlate;
            }
            else
            {
                SellingDataTemPlate.SetBinding(SellingViewCell.PostModelProperty, new Binding(nameof(PostModel)){ Source = this });
                SellingDataTemPlate.SetBinding(SellingViewCell.ContainerBindingContextProperty, new Binding(nameof(ContainerBindingObject)){  Source = this });
                return SellingDataTemPlate;
            }
        }
    }
}