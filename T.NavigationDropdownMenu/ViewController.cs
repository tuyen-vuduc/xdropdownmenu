using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

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

			SetupMenuView ();

			Title = "NavigationDropdownMenu";
		}

		private void SetupMenuView() {
			var items = new string[] {"Most Popular", "Latest", "Trending", "Nearest", "Top Picks"};

			this.NavigationController.NavigationBar.Translucent = false;
			this.NavigationController.NavigationBar.BarTintColor = new UIColor (
				(nfloat)(0.0 / 255.0), 
				(nfloat)(180 / 255.0), 
				(nfloat)(220 / 255.0), 
				(nfloat)1.0
			);

			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes {
				ForegroundColor = UIColor.White
			};

			var menuView = new DropdownMenuView(
				new CGRect(0.0, 0.0, 300, 44), 
				items.First(), 
				items, 
				this.View);
			
			menuView.CellHeight = 50;
			menuView.CellBackgroundColor = this.NavigationController.NavigationBar.TintColor;

			menuView.CellSelectionColor = new UIColor(
				(nfloat)(0.0/255.0), 
				(nfloat)(160.0/255.0), 
				(nfloat)(195.0/255.0), 
				(nfloat)1.0
			);
			
			menuView.CellTextLabelColor = UIColor.White;
			menuView.CellTextLabelFont = UIFont.FromName(name: "Avenir-Heavy", size: 17);
			menuView.ArrowPadding = 15;
			menuView.AnimationDuration = 0.3;
			menuView.MaskBackgroundColor = UIColor.Black;
			menuView.MaskBackgroundOpacity = (nfloat)0.3;
			menuView.BounceOffset = 5;


			this.NavigationItem.TitleView = menuView;
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

