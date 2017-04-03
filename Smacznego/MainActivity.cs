using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace Smacznego
{
	[Activity (Label = "Smacznego", MainLauncher = true, Icon = "@drawable/Icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Baza.getDataBase().setApplicationContext(this.ApplicationContext);

			SetContentView (Resource.Layout.MenuGlowne);

			Button czysc = FindViewById<Button> (Resource.Id.MA_Pczysc);
			Button dodaj = FindViewById<Button> (Resource.Id.MA_Pdodaj);
			TextView pole_szukania = FindViewById<TextView> (Resource.Id.MA_poleszukaj);
			ListView lista = FindViewById <ListView>(Resource.Id.MA_ListaPrzepisow);

			lista.Adapter = Baza.getDataBase().getBaseAdapter();
			lista.RequestFocus ();

			czysc.Visibility = Android.Views.ViewStates.Invisible;

			czysc.Click += delegate {
				pole_szukania.SetText ("", TextView.BufferType.Normal);
			};

			dodaj.Click += delegate {
				FragmentTransaction trans = FragmentManager.BeginTransaction();
				dialogAddRecipe addRecipe = new dialogAddRecipe();
				addRecipe.Show(trans,"addRecipe");

				addRecipe.recipeEvent += delegate(object sender, onRecipeAddRequest e) {
					Baza.getDataBase().dodajPrzepis(e.Przepis);
				};
			};

			pole_szukania.TextChanged += delegate {
				if(pole_szukania.Text != "")
					czysc.Visibility = Android.Views.ViewStates.Visible;
				else
					czysc.Visibility = Android.Views.ViewStates.Invisible;
				Baza.getDataBase().przeladujPrzepisy(pole_szukania.Text);
			};

			lista.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				Intent intencja = new Intent(this,typeof(PrzepisActivity));
				intencja.PutExtra("Id",e.Position);
				this.StartActivity(intencja);
				//this.OverridePendingTransition(Android.Resource.Animation.FadeIn,Android.Resource.Animation.FadeOut);
			};

			lista.ItemLongClick += delegate(object sender, AdapterView.ItemLongClickEventArgs e) {
				FragmentTransaction trans = FragmentManager.BeginTransaction();
				dialogShowIngredients showig = new dialogShowIngredients(Baza.getDataBase().getBaseAdapter()[e.Position].Skladniki);
				showig.Show(trans,"showIngredients");
			};
				
		}
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			Baza.getDataBase ().wylaczBaze ();
			GC.Collect ();
		}
	}
}