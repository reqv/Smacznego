using System;
using SQLite;
using Android.Database.Sqlite;
using System.Collections.Generic;
using Android.Content;
using System.Collections.ObjectModel;
using Android.Widget;
using Android.Graphics;

namespace Smacznego
{
	public class Baza
	{
		private SQLite.SQLiteConnection conn = null;
		private string folder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
		private string fileName = "SmacznegoPrzepisy.db";
		private PrzepisyAdapter adapter;
		private List<TabelaPrzepisy> aktualnePrzepisy;
		private Context applicationContext = null;

		private static Baza holder = new Baza();
		public static Baza getDataBase() {return holder;}

		private Baza()
		{
			restoreDatabase();
		}
			
		public void setApplicationContext(Context context)
		{
			applicationContext = context;
			aktualnePrzepisy = pobierzBazePrzepisow ();
			adapter = new PrzepisyAdapter (applicationContext, aktualnePrzepisy);
		}

		public string getRecipeType(int typeId)
		{
			return applicationContext.Resources.GetStringArray (Resource.Array.Recipe_Types)[typeId];
		}

		private void restoreDatabase()
		{
			conn = new SQLiteConnection (System.IO.Path.Combine (folder, fileName));
			conn.CreateTable<TabelaPrzepisy> ();
		}

		public void dodajPrzepis (TabelaPrzepisy nowyPrzepis)
		{
			if (conn == null)
				restoreDatabase ();
			try{
				conn.Insert (nowyPrzepis);
			}catch(Exception ex){
				Toast.MakeText(applicationContext,applicationContext.GetString(Resource.String.databaseAddError)+ex.Message,ToastLength.Short).Show();
				return;
			}
			przeladujPrzepisy();
			Toast.MakeText(applicationContext,Resource.String.databaseAdd,ToastLength.Short).Show();
		}

		public void zmienObrazek (int id, byte[] obrazek)
		{
			TabelaPrzepisy prze = conn.Get<TabelaPrzepisy> (id);
			prze.Obrazek = obrazek;
			conn.Update (prze);
			przeladujPrzepisy ();
		}

		/*public void zmienObrazek2 (int id, Android.Net.Uri uri)
		{
			TabelaPrzepisy prze =  conn.Get<TabelaPrzepisy> (id);
			prze.Obrazek = uri.ToString ();
			conn.Update (prze);
			przeladujPrzepisy();
		}

		public Android.Net.Uri getImageUri (int id)
		{
			Android.Net.Uri urifrombase = null;
			TabelaPrzepisy prze =  conn.Get<TabelaPrzepisy> (id);
			if (prze.Obrazek == null)
				return urifrombase;
			urifrombase = Android.Net.Uri.Parse (prze.Obrazek);
			return urifrombase;
		}*/

		private List<TabelaPrzepisy> pobierzBazePrzepisow()
		{
			if (conn == null)
				restoreDatabase ();
			List<TabelaPrzepisy> nowaLista = new List<TabelaPrzepisy>();
			var query = conn.Table<TabelaPrzepisy> ();
			foreach (var przepis in query) 
				nowaLista.Add (przepis);
			return nowaLista;
		}

		public void przeladujPrzepisy(String warunek = "")
		{
			if (conn == null)
				restoreDatabase ();
			TableQuery<TabelaPrzepisy> query = null;
			if (warunek.CompareTo ("") != 0)
				query = conn.Table<TabelaPrzepisy>().Where (v => v.Nazwa.Contains(warunek));
			else
				query = conn.Table<TabelaPrzepisy> ();
			aktualnePrzepisy.Clear ();
			foreach (var przepis in query)
				aktualnePrzepisy.Add (przepis);

			adapter.NotifyDataSetChanged ();
		}

		public void usunPrzepis(int id)
		{
			if (conn == null)
				restoreDatabase ();
			try{
			conn.Delete<TabelaPrzepisy> (id);
			}
			catch(Exception ex) {
				Toast.MakeText (applicationContext, applicationContext.GetString(Resource.String.databaseDeleteError) + ex.Message, ToastLength.Short);
			}
			przeladujPrzepisy ();
		}

		public PrzepisyAdapter getBaseAdapter()
		{
			return adapter;
		}

		public void wylaczBaze()
		{
			if (conn == null)
				return;
			conn.Close ();
			conn = null;
		}
	}
}
