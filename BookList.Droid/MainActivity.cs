using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using SQLite;
using System.Collections.Generic;

namespace BookList.Droid
{
	[Activity (Label = "BookList.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		EditText txtTitle;
		EditText txtISBN;
		Button btnAddBook;
		ListView tblBooks;

		// define the file path on the device where the DB will be stored
		string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "BookList.db3");

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			txtTitle = FindViewById<EditText> (Resource.Id.txtTitle);
			txtISBN = FindViewById<EditText> (Resource.Id.txtISBN);
			btnAddBook = FindViewById<Button> (Resource.Id.btnAddBook);
			tblBooks = FindViewById<ListView> (Resource.Id.tblBooks);
			// Create our connection, if the database file and/or table doesn't exist create it
			//TODO: Add DB Creation  


			// create the database file if it doesn't exist  
			try  
			{     // Create our connection, if the database and/or table doesn't exist create it          
				var db = new SQLiteConnection(filePath);     
				db.CreateTable<Book>();   
			}   
			catch (IOException ex)   
			{  
				var reason = string.Format("Failed to create Table - reason {0}", ex.Message);  
				Toast.MakeText(this, reason, ToastLength.Long).Show();    
			}  
			// TODO: Add code to populate the Table View if it contains data
			PopulateListView(); 

			btnAddBook.Click += BtnAddBook_Click;

		}

		void BtnAddBook_Click (object sender, System.EventArgs e)
		{
			string alertTitle, alertMessage;
			//input Validation: onliy insert a book if the title is not mpty
			if (!string.IsNullOrEmpty (txtTitle.Text)) {
				//Insert a book into the database 
				var newBook = new Book { BookTitle = txtTitle.Text, ISBN = txtISBN.Text };

				var db = new SQLiteConnection (filePath);
				db.Insert (newBook);
				//show an alert to confirm that the book has been added
				alertTitle = "Success";
				alertMessage = string.Format ("Book ID: {0} with Title: {1} has been successfully added!",
					newBook.BookId, newBook.BookTitle);
				//TODO: Add code to populate the List View with the new values
				PopulateListView(); 
			} else { // show failed alert message
				alertTitle = "Failed";
				alertMessage = "Enter a valid Book Title";
			}
			//create an alert and show it based on teh alet title and message created earlier
			AlertDialog.Builder alert = new AlertDialog.Builder (this);
			alert.SetTitle (alertTitle);
			alert.SetMessage (alertMessage);
			alert.SetPositiveButton ("OK", (senderAlert, args) => {
				Toast.MakeText (this, "Continue!", ToastLength.Short).Show ();
			});
			alert.SetNegativeButton ("Cancel", (senderAlert, args) => {
				Toast.MakeText (this, "Canceled!", ToastLength.Short).Show ();
			});
			Dialog dialog = alert.Create ();
			dialog.Show ();
		}

		private void PopulateListView()
		{
			var db = new SQLiteConnection (filePath);
			//retrieve all the data in the DB table
			var bookList = db.Table<Book>();

			List<string> bookTitles = new List<string> ();

			//loop through the data and retrieve teh bookTitle column data only
			foreach (var book in bookList) {
				bookTitles.Add (book.BookTitle);
			}
			// set the data source / Adapter for the listView control to an array of the retrieved books
			tblBooks.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1,bookTitles.ToArray ());
		} 
	}
}


