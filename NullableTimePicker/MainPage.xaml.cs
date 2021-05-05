using System;
using Xamarin.Forms;

namespace NullableTimePicker
{
    public partial class MainPage : ContentPage
    {
        public TimeSpan? MyTime { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
