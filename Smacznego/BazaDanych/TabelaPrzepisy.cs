using System;
using SQLite;

namespace Smacznego
{
	public class TabelaPrzepisy
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[MaxLength(30),NotNull,Unique]
		public string Nazwa { get; set; }

		[NotNull]
		public int Typ{ get; set; }
		
		[NotNull]
		public string Skladniki{ get; set; }

		[NotNull]
		public string Przepis{ get; set; }

		public byte[] Obrazek{ get; set; }

		public string Porady{ get; set; }
	}
}
