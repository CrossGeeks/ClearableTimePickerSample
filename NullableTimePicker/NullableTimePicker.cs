using System;
using Xamarin.Forms;

namespace NullableTimePicker.Controls
{
	public class NullableTimePicker : TimePicker
	{
		public NullableTimePicker() { Format = _originalFormat = "hh:mm tt"; }
		
		public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(NullableTimePicker), "/ . /");
		public static readonly BindableProperty NullableTimeProperty = BindableProperty.Create(nameof(NullableTime), typeof(TimeSpan?), typeof(NullableTimePicker), null, defaultBindingMode: BindingMode.TwoWay);

		public string PlaceHolder
		{
			get => (string)GetValue(PlaceHolderProperty); 
			set => SetValue(PlaceHolderProperty, value);
		}

		public TimeSpan? NullableTime
		{
			get => (TimeSpan?)GetValue(NullableTimeProperty); 
			set { SetValue(NullableTimeProperty, value); UpdateTimeFormat(); }
		}

		public string _originalFormat { get; private set; } = null;

		public void ClearTime()
		{
			NullableTime = null;
			UpdateTimeFormat();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
			{
				_originalFormat = Format;
				UpdateTimeFormat();
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == TimeProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused))
			{
				NullableTime = Time;
				UpdateTimeFormat();
			}
		}

		private void UpdateTimeFormat()
		{
			if (NullableTime != null)
			{
				Format = _originalFormat;
			}
			else
			{
				Format = PlaceHolder;
			}
		}
	}
}
