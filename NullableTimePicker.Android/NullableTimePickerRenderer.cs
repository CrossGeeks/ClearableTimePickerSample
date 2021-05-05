using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Widget;
using System.ComponentModel;
using NullableTimePicker.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(NullableTimePicker.Controls.NullableTimePicker), typeof(NullableTimePickerRenderer))]
namespace NullableTimePicker.Droid
{
    public class NullableTimePickerRenderer : ViewRenderer<Controls.NullableTimePicker, EditText>
    {
        TimePickerDialog _dialog;

        public NullableTimePickerRenderer(Context context) : base(context){ }

        protected override void OnElementChanged(ElementChangedEventArgs<Controls.NullableTimePicker> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(new Android.Widget.EditText(Context));

            if (Control == null || e.NewElement == null)
                return;
            
            Control.Click += OnPickerClick;
            Control.KeyListener = null;
            Control.FocusChange += OnPickerFocusChange;
            Control.Enabled = Element.IsEnabled;
            Control.Text = !Element.NullableTime.HasValue ? Element.PlaceHolder : DateTime.Today.Add(Element.NullableTime.Value).ToString(Element.Format);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.TimePicker.TimeProperty.PropertyName || e.PropertyName == Xamarin.Forms.TimePicker.FormatProperty.PropertyName)
            {
                if (Element.Format == Element.PlaceHolder)
                {
                    this.Control.Text = Element.PlaceHolder;
                    return;
                }
            }
            
            if (e.PropertyName == NullableTimePicker.Controls.NullableTimePicker.NullableTimeProperty.PropertyName && Control != null && Element != null)
            {
                Control.Text = !Element.NullableTime.HasValue ? Element.PlaceHolder : DateTime.Today.Add(Element.NullableTime.Value).ToString(Element.Format);
            }

            base.OnElementPropertyChanged(sender, e);
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowTimePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Click -= OnPickerClick;
                Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        void ShowTimePicker()
        {
            CreateTimePickerDialog(Element.NullableTime.HasValue? Element.NullableTime.Value.Hours : 0, Element.NullableTime.HasValue ? Element.NullableTime.Value.Minutes : 0, false);
            _dialog.Show();
        }

        void CreateTimePickerDialog(int hours, int minutes, bool is24HourView)
        {
            _dialog = new TimePickerDialog(Context, (o, e) =>
            {
                SetTime(new TimeSpan(e.HourOfDay, e.Minute, 0));
                ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, hours, minutes, is24HourView);

            _dialog.SetButton2("Clear", (sender, e) =>
            {
                this.Element.ClearTime();
                Control.Text = this.Element.Format;
            });
        }

        void SetTime(TimeSpan time)
        {
            Element.Format = this.Element._originalFormat;
            Control.Text = DateTime.Today.Add(time).ToString(Element.Format);
            Element.Time = time;
        }
    }
}