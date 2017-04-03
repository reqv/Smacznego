using System;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Graphics;
using Android.Content.Res;

namespace Smacznego
{
	public class PrzepisyAdapter : BaseAdapter<TabelaPrzepisy>
	{
		private List<TabelaPrzepisy> lista;
		private Context kontekst;

		public PrzepisyAdapter(Context context,List<TabelaPrzepisy> items)
		{
			lista = items;
			kontekst = context;
		}

		public override int Count {
			get {
				return lista.Count;
			}
		}
		public override long GetItemId (int position)
		{
			return position;
		}

		public override TabelaPrzepisy this[int position]
		{
			get{ return lista [position]; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null)
			{
				row = LayoutInflater.From (kontekst).Inflate(Resource.Layout.ListRow,null,false);
			}

			TextView Name = row.FindViewById<TextView> (Resource.Id.ROW_Nazwa);
			TextView Typ = row.FindViewById<TextView> (Resource.Id.ROW_Typ);
			ImageView Image = row.FindViewById<ImageView> (Resource.Id.ROW_Obrazek);

			switch(lista[position].Typ)
			{
			case 0:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_danie);
				break;
			case 1:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_weget);
				break;
			case 2:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_zupa);
				break;
			case 3:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_deser);
				break;
			case 4:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_napoj);
				break;
			default:
				Image.SetImageResource (Resource.Drawable.IkonaPrzepisu_danie);
				break;
			}

			Name.Text = lista [position].Nazwa;
			Typ.Text = Baza.getDataBase ().getRecipeType (lista [position].Typ);

			return row;
		}
	}
}

