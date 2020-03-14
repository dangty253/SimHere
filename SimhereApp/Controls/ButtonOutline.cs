using System;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace SimhereApp.Portable.Controls
{
    public class ButtonOutline : RadButton
    {
        public static readonly BindableProperty OutlineColorProperty =
            BindableProperty.Create(nameof(OutlineColor),
                typeof(Color),
                typeof(CurrencyEntry),
                Color.Transparent,
                BindingMode.TwoWay);
        public Color OutlineColor { get => (Color)GetValue(OutlineColorProperty); set => SetValue(OutlineColorProperty, value); }
        public ButtonOutline()
        {
            HeightRequest = 40;
            BackgroundColor = Color.White;
            BorderWidth = 1;
            CornerRadius = 20;

            this.SetBinding(Button.TextColorProperty, new Binding("OutlineColor")
            {
                Source = this
            });
            this.SetBinding(Button.BorderColorProperty, new Binding("OutlineColor")
            {
                Source = this
            });
        }
    }
}
