
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using System.Threading;
using Android.Util;

namespace Smacznego
{
	[Activity (Label = "PrzepisActivity")]
	public class PrzepisActivity : Activity
	{
		private TabelaPrzepisy przepis;
		private int id;
		ImageView RecImage;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Przepis);

			id = Intent.GetIntExtra("Id",-1);
			if (id == -1)
				Finish ();
			else {
				przepis = Baza.getDataBase ().getBaseAdapter () [id];
			}
		
			TextView Title = FindViewById<TextView> (Resource.Id.P_title);
			TextView Igridience = FindViewById<TextView> (Resource.Id.P_skladniki);
			TextView Recipe = FindViewById<TextView> (Resource.Id.P_przepis);

			Title.Text = przepis.Nazwa;
			Igridience.Text = przepis.Skladniki;
			Recipe.Text = przepis.Przepis;

			RecImage = FindViewById<ImageView> (Resource.Id.P_obrazek);

			ThreadPool.QueueUserWorkItem (o => LoadImage());

			RecImage.LongClick += delegate{
				Intent pobierzObrazek = new Intent();
				pobierzObrazek.SetType("image/*");
				pobierzObrazek.SetAction(Intent.ActionGetContent);
				this.StartActivityForResult(Intent.CreateChooser(pobierzObrazek,GetString(Resource.String.P_chooseImage)),0);
			};

			Button DeleteBut = FindViewById<Button> (Resource.Id.P_deleteRecipe);

			DeleteBut.Click += delegate {
				FragmentTransaction trans = FragmentManager.BeginTransaction();
				dialogDelete addRecipe = new dialogDelete(przepis.Id);
				addRecipe.Show(trans,"deleteRecipe");
			};
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				Toast.MakeText (this, Resource.String.P_imageUpload, ToastLength.Short).Show();
				ThreadPool.QueueUserWorkItem (o => zapiszObrazek(data.Data,150,150,przepis.Id));
			}

		}

		public void LoadImage()
		{
			if (przepis.Obrazek == null)
				return;
			try{
				Bitmap bitmapa = BitmapFactory.DecodeByteArray(przepis.Obrazek,0,przepis.Obrazek.Length);
				RunOnUiThread (() => RecImage.SetImageBitmap(bitmapa));
				System.GC.Collect();
			}catch(Exception ex){
				RunOnUiThread (() => Toast.MakeText (this, ex.Message, ToastLength.Short).Show());
			}
		}

		private void zapiszObrazek(Android.Net.Uri data,int requestedWidth,int requestedHeight,int id)
		{
			try{
				Stream stream = ContentResolver.OpenInputStream (data);
				BitmapFactory.Options options = new BitmapFactory.Options ();

				//Options
				options.InJustDecodeBounds = true;
				options.InPurgeable = true;
				options.InPreferredConfig = Bitmap.Config.Rgb565;
				options.InInputShareable = true;
				options.InDither = false;

				//praca
				BitmapFactory.DecodeStream (stream);
				options.InSampleSize = calculateSampleSize (options, requestedWidth, requestedHeight);
				stream = ContentResolver.OpenInputStream (data);
				options.InJustDecodeBounds = false;
				Bitmap bitmap = BitmapFactory.DecodeStream (stream, null, options);
				MemoryStream mstream = new MemoryStream();
				bitmap.Compress (Bitmap.CompressFormat.Webp, 80, mstream);

				RunOnUiThread (() => Baza.getDataBase ().zmienObrazek (id, mstream.ToArray()));	//zapis do bazy

				//czystka
				stream = null;
				RunOnUiThread (() => Toast.MakeText(this,Resource.String.P_imageUploadComplete,ToastLength.Short).Show());
				RunOnUiThread (() => RecImage.SetImageBitmap(bitmap));
				Thread.Sleep(1000);
				mstream.SetLength (0);
				System.GC.Collect ();
			}catch(Exception ex){
				RunOnUiThread (() => Toast.MakeText(this,GetString(Resource.String.P_imageUploadError)+ex.Message,ToastLength.Short).Show());
			}
		}
		private static int calculateSampleSize(BitmapFactory.Options options, int requestedWidth,int requestedHeight)
		{
			int actualWidth = options.OutWidth;
			int actualHeight = options.OutHeight;

			double inSampleSize = 1D;

			if (actualWidth > requestedWidth || actualHeight > requestedHeight) {
				int halfWidth = actualWidth / 2;
				int halfHeight = actualHeight / 2;
				while ((halfWidth / inSampleSize) > requestedWidth && (halfHeight / inSampleSize) > requestedHeight) {
					inSampleSize *= 2;
				}
			}
			return (int)inSampleSize;
		}

		protected override void OnDestroy ()
		{
			RecImage.Dispose ();
			base.OnDestroy ();
			System.GC.Collect ();
		}
	}
}