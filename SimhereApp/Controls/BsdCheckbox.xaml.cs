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
    public partial class BsdCheckbox : ContentView
    {
        public BsdCheckbox()
        {
            InitializeComponent();

            MainContainer.BindingContext = this;
            checkBox.BindingContext = this;
            title.BindingContext = this;

            MainContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(tapped)
            });

        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(BsdCheckbox), false, BindingMode.TwoWay, propertyChanged: checkedPropertyChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(BsdCheckbox), "", BindingMode.OneWay);
        public static readonly BindableProperty CheckedChangedCommandProperty = BindableProperty.Create(nameof(CheckedChangedCommand), typeof(Command), typeof(BsdCheckbox), null, BindingMode.OneWay);
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(BsdCheckbox), null, BindingMode.OneWay);
        public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(BsdCheckbox), Color.Black, BindingMode.OneWay);
        public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(BsdCheckbox), Color.DarkSlateGray, BindingMode.OneWay);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(BsdCheckbox), Color.Black, BindingMode.OneWay);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(BsdCheckbox), 1.0, BindingMode.OneWay);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public Command CheckedChangedCommand
        {
            get { return (Command)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public Label ControlLabel
        {
            get { return title; }
        }

        public Color UncheckedColor
        {
            get { return (Color)GetValue(UncheckedColorProperty); }
            set { SetValue(UncheckedColorProperty, value); }
        }

        public Color CheckedColor
        {
            get { return (Color)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }

        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        static void checkedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((BsdCheckbox)bindable).ApplyCheckedState();

        }

        /// <summary>  
        /// Handle chackox tapped action  
        /// </summary>  
        void tapped()
        {
            IsChecked = !IsChecked;
            ApplyCheckedState();
            this.SendChangeChecked();
        }

        /// <summary>  
        /// Reflect the checked event change on the UI  
        /// with a small animation  
        /// </summary>  
        /// <param name="isChecked"></param>  
        ///   
        void ApplyCheckedState()
        {
            checkBox.IsChecked = IsChecked;
        }

        public event EventHandler changeChecked;
        public void SendChangeChecked()
        {
            EventHandler eventHandler = this.changeChecked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}