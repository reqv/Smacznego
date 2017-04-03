using System;
using Android.App;
using Android.Widget;

namespace Smacznego
{
	public class onRecipeAddRequest : EventArgs
	{
		private TabelaPrzepisy pprzepis;
		public TabelaPrzepisy Przepis
		{
			get{ return pprzepis; }
			set{ pprzepis = value; }
		}

		public onRecipeAddRequest(TabelaPrzepisy nowyprzepis) : base()
		{
			Przepis = nowyprzepis;
		}
	}

	public class dialogAddRecipe : DialogFragment
	{
		public event EventHandler<onRecipeAddRequest> recipeEvent;
		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var widok = inflater.Inflate (Resource.Layout.dialogAddRecipe, container, false);

			Button dodajPrzepisBut = widok.FindViewById<Button> (Resource.Id.AR_dodajPrzepis);
			TextView nazwa = widok.FindViewById<TextView> (Resource.Id.AR_nazwa);
			Spinner typ = widok.FindViewById<Spinner> (Resource.Id.AR_typ);
			TextView sklad = widok.FindViewById<TextView> (Resource.Id.AR_skladniki);
			TextView przepis = widok.FindViewById<TextView> (Resource.Id.AR_przepis);

			var adapter = ArrayAdapter.CreateFromResource (this.Activity.ApplicationContext, Resource.Array.Recipe_Types,Resource.Layout.spinner_item);

			adapter.SetDropDownViewResource(Resource.Layout.spinner_dropdown_item);
			typ.Adapter = adapter;

			dodajPrzepisBut.Click += delegate {
				if(nazwa.Text.Length == 0)
				{
					nazwa.SetError(GetString(Resource.String.AR_NameError),null);
					nazwa.RequestFocus();
					return;
				}
				if(sklad.Text.Length == 0)
				{
					sklad.SetError(GetString(Resource.String.AR_IgriError),null);
					sklad.RequestFocus();
					return;
				}
				if(przepis.Text.Length == 0)
				{
					przepis.SetError(GetString(Resource.String.AR_RecipeError),null);
					przepis.RequestFocus();
					return;
				}
				TabelaPrzepisy nowyPrzepis = new TabelaPrzepisy{ Nazwa = nazwa.Text, Skladniki = sklad.Text, 
					Przepis = przepis.Text, Typ = (int)typ.SelectedItemId,Obrazek = null };
				recipeEvent.Invoke(this,new onRecipeAddRequest(nowyPrzepis));
				this.Dismiss();
			};

			return widok;
		}
		public override void OnActivityCreated (Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
		}
	}
}

