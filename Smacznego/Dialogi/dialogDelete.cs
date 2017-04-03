using System;
using Android.App;
using Android.Widget;

namespace Smacznego
{
	public class dialogDelete : DialogFragment
	{
		private int id;
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var widok = inflater.Inflate (Resource.Layout.dialogDelete, container, false);

			Button ok = widok.FindViewById<Button> (Resource.Id.DD_ok);
			Button cancel = widok.FindViewById<Button> (Resource.Id.DD_cancel);

			ok.Click += delegate {
				Baza.getDataBase().usunPrzepis(id);
				this.Activity.Finish();
			};

			cancel.Click += delegate {
				this.Dismiss();
			};

			return widok;
		}

		public override void OnActivityCreated (Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
		}

		public dialogDelete(int _id)
		{
			id = _id;
		}
	}
}