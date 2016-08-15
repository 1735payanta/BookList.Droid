using System;
using SQLite;

namespace BookList.Droid
{
	public class Book
	{
		[PrimaryKey, AutoIncrement]
		public int BookId { get; set; }
		public string BookTitle { get; set; }
		public string ISBN { get; set; }


	}
}

