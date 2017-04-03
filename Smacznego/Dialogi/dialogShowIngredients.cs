using System;
using Android.App;
using Android.Widget;

namespace Smacznego
{
	public class dialogShowIngredients : DialogFragment
	{
		private string Skladniki;
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var widok = inflater.Inflate (Resource.Layout.dialogShowIngredients, container, false);

			Button ok = widok.FindViewById<Button> (Resource.Id.SI_ok);
			TextView sklView = widok.FindViewById<TextView> (Resource.Id.SI_whatneeded);

			sklView.Text = Skladniki;

			ok.Click += delegate {
				this.Dismiss();
			};

			return widok;
		}

		public override void OnActivityCreated (Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
		}

		public dialogShowIngredients(string skladniki)
		{
			Skladniki = skladniki;
		}
	}
}

