using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace T.NavigationDropdownMenu
{
	public partial class ViewController : UITableViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			var items = new List<string> ();
			for (int i = 0; i < 50; i++) {
				items.Add ("NavigationDropdownMenu " + i);
			}

			TableView.Source = new SampleSource (items);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}

	public class SampleSource : UITableViewSource {

		private readonly IEnumerable<string> items;

		public SampleSource (IEnumerable<string> items)
		{
			this.items = items;
		}

		#region implemented abstract members of UITableViewSource
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("sample");

			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Value1, "sample");
			}

			cell.TextLabel.Text = items.ElementAt (indexPath.Row);

			return cell;
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return (nint) items.Count ();
		}
		#endregion
		
	}
}

