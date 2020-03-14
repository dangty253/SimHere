using SimhereApp.Portable.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyEntry : ContentView
    {
        #region MaxValue
        public static readonly BindableProperty MaxValueProperty =
            BindableProperty.Create(nameof(MaxValue),
                typeof(decimal),
                typeof(CurrencyEntry),
                0m,
                BindingMode.TwoWay);
        public decimal MaxValue { get => (decimal)GetValue(MaxValueProperty); set => SetValue(MaxValueProperty, value); }
        #endregion

        #region Placeholder
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder),
                typeof(string),
                typeof(CurrencyEntry),
                "",
                BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        #endregion


        #region Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text),
                typeof(decimal?),
                typeof(CurrencyEntry),
                null,
                BindingMode.TwoWay);
        public decimal? Text { get => (decimal?)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        #endregion

        public CurrencyEntry()
        {
            InitializeComponent();
            this.entry.SetBinding(Entry.TextProperty, new Binding("Text")
            {
                Source = this,
                Converter = new CurrencyEntryConverter()
            });
            this.entry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder")
            {
                Source = this
            });
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text > MaxValue)
            {
                var oldText = e.OldTextValue;
                if (oldText != null && string.IsNullOrEmpty(oldText) == false)
                {
                    Text = decimal.Parse(oldText.Replace(".", ""));
                }
                else
                {
                    Text = null;
                }
            }
        }
    }
}