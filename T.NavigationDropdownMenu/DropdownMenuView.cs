using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using System.Linq;

namespace T.NavigationDropdownMenu
{
	public class DropdownMenuView: UIView {
		public event EventHandler<ItemSelectedEventArgs> MenuSelected;

		nfloat cellHeight;
		public nfloat CellHeight {
			get {
				return cellHeight;
			}
			set {
				cellHeight = value;
				this.configuration.CellHeight = cellHeight;
			}
		}

		UIColor cellBackgroundColor;
		public UIColor CellBackgroundColor {
			get {
				return cellBackgroundColor;
			}
			set {
				cellBackgroundColor = value;
				this.configuration.CellBackgroundColor = cellBackgroundColor;
			}
		}

		UIColor cellTextLabelColor;
		public UIColor CellTextLabelColor {
			get {
				return cellTextLabelColor;
			}
			set {
				cellTextLabelColor = value;
				this.configuration.CellTextLabelColor = cellTextLabelColor;
			}
		}

		UIFont cellTextLabelFont;
		public UIFont CellTextLabelFont {
			get {
				return cellTextLabelFont;
			}
			set {
				cellTextLabelFont = value;

				this.configuration.CellTextLabelFont = cellTextLabelFont;
				this.menuTitle.Font = cellTextLabelFont;
			}
		}

		UIColor cellSelectionColor;
		public UIColor CellSelectionColor {
			get {
				return cellSelectionColor;
			}
			set {
				cellSelectionColor = value;
				this.configuration.CellSelectionColor = cellSelectionColor;
			}
		}

		UIImage checkMarkImage;
		public UIImage CheckMarkImage {
			get {
				return checkMarkImage;
			}
			set {
				checkMarkImage = value;
				this.configuration.CheckMarkImage = checkMarkImage;
			}
		}

		double animationDuration;
		public double AnimationDuration {
			get {
				return animationDuration;
			}
			set {
				animationDuration = value;
				this.configuration.AnimationDuration = animationDuration;
			}
		}

		nfloat bounceOffset;
		public nfloat BounceOffset {
			get {
				return bounceOffset;
			}
			set {
				bounceOffset = value;
				this.configuration.BounceOffset = bounceOffset;
			}
		}

		UIImage arrowImage;
		public UIImage ArrowImage {
			get {
				return arrowImage;
			}
			set {
				arrowImage = value;
				this.configuration.ArrowImage = arrowImage;
				this.menuArrow.Image = arrowImage;
			}
		}

		nfloat arrowPadding;
		public nfloat ArrowPadding {
			get {
				return arrowPadding;
			}
			set {
				arrowPadding = value;
				this.configuration.ArrowPadding = arrowPadding;
			}
		}

		UIColor maskBackgroundColor;
		public UIColor MaskBackgroundColor {
			get {
				return maskBackgroundColor;
			}
			set {
				maskBackgroundColor = value;
				this.configuration.MaskBackgroundColor = maskBackgroundColor;
			}
		}

		nfloat maskBackgroundOpacity;
		public nfloat MaskBackgroundOpacity {
			get {
				return maskBackgroundOpacity;
			}
			set {
				maskBackgroundOpacity = value;
				this.configuration.MaskBackgroundOpacity = maskBackgroundOpacity;
			}
		}

		// Private properties
		private UIView tableContainerView;
		private DropdownMenuConfiguration configuration;
		private CGRect mainScreenBounds;
		private UIButton menuButton;
		private UILabel menuTitle;
		private UIImageView menuArrow;
		private UIView backgroundView;
		private DropdownMenuTableView tableView;
		private IEnumerable<object> items;
		private bool isShown;
		private nfloat navigationBarHeight;

		public DropdownMenuView (CGRect frame, string title, IEnumerable<string> items, UIView containerView) : base(frame)
		{
			// Init properties
			this.configuration = new DropdownMenuConfiguration();
			this.tableContainerView = containerView;
			this.navigationBarHeight = 44;
			this.mainScreenBounds = UIScreen.MainScreen.Bounds;
			this.isShown = false;
			this.items = items;

				// Init button as navigation title
			this.menuButton = new UIButton(frame);
			this.menuButton.TouchUpInside += MenuButton_TouchUpInside;;
			this.AddSubview (this.menuButton);

			this.menuTitle = new UILabel (frame);
			this.menuTitle.Text = title;
			this.menuTitle.TextColor = UINavigationBar.Appearance.TitleTextAttributes != null ? 
				UINavigationBar.Appearance.TitleTextAttributes.ForegroundColor : null;
			this.menuTitle.TextAlignment = UITextAlignment.Center;
			this.menuTitle.Font = this.configuration.CellTextLabelFont;
			this.menuButton.AddSubview (this.menuTitle);

			this.menuArrow = new UIImageView (this.configuration.ArrowImage);
			this.menuButton.AddSubview (this.menuArrow);

			// Init table view
			this.tableView = new DropdownMenuTableView(new CGRect(mainScreenBounds.X, mainScreenBounds.Y, mainScreenBounds.Width, mainScreenBounds.Height + 300 - 64), items, this.configuration);
			this.tableView.ItemSelected += (sender, e) => {
				if (this.MenuSelected != null) {
					this.MenuSelected(this, e);
				}

				this.menuTitle.Text = e.Item as string;
				this.HideMenu();
				this.isShown = false;
				this.LayoutSubviews();
			};
		}

		void MenuButton_TouchUpInside (object sender, EventArgs e)
		{
			this.isShown = !this.isShown;

			if (this.isShown == true) {
				this.ShowMenu();
			} else {
				this.HideMenu();
			}
		}

		public override void LayoutSubviews ()
		{
			this.menuTitle.SizeToFit ();
			this.menuTitle.Center = new CGPoint (this.Frame.Size.Width / 2, this.Frame.Size.Height / 2);
			this.menuArrow.SizeToFit ();
			this.menuArrow.Center = new CGPoint (
				this.menuTitle.Frame.GetMaxX () + this.configuration.ArrowPadding, 
				this.Frame.Size.Height / 2);
		}

		public void ShowMenu() {
			// Table view header
			var headerView = new UIView(new CGRect(0, 0, this.Frame.Width, 300));
			headerView.BackgroundColor = this.configuration.CellBackgroundColor;
			this.tableView.TableHeaderView = headerView;

			// Reload data to dismiss highlight color of selected cell
			this.tableView.ReloadData();

			// Init background view (under table view)
			this.backgroundView = new UIView(mainScreenBounds);
			this.backgroundView.BackgroundColor = this.configuration.MaskBackgroundColor;

			// Add background view & table view to container view
			this.tableContainerView.AddSubview(this.backgroundView);
			this.tableContainerView.AddSubview(this.tableView);

			// Rotate arrow
			this.RotateArrow();

			// Change background alpha
			this.backgroundView.Alpha = 0;

			// Animation
			this.tableView.Frame = new CGRect(
				this.tableView.Frame.X,
				(nfloat) (-this.items.Count() * this.configuration.CellHeight - 300),
				this.tableView.Frame.Width,
				this.tableView.Frame.Height
			);

			UIView.Animate (this.configuration.AnimationDuration, () => {
				this.tableView.Frame = new CGRect(
					this.tableView.Frame.X,
					-300,
					this.tableView.Frame.Width,
					this.tableView.Frame.Height
				);

				this.tableView.ContentOffset = new CGPoint(0, -this.configuration.BounceOffset);

				this.backgroundView.Alpha = this.configuration.MaskBackgroundOpacity;
			}, () => {
				UIView.Animate(this.configuration.AnimationDuration/3, () => {
					this.tableView.ContentOffset = new CGPoint(0, 0);
				});
			});
		}

		private void HideMenu() {
			// Rotate arrow
			this.RotateArrow();

			// Change background alpha
			this.backgroundView.Alpha = this.configuration.MaskBackgroundOpacity;

			// Animation
			UIView.Animate(
				this.configuration.AnimationDuration,
				delay: (nfloat)0.15,
				options: UIViewAnimationOptions.TransitionNone,
				animation: () => {
					this.tableView.Frame = new CGRect(
						this.tableView.Frame.X,
						(nfloat)(- this.items.Count() * this.configuration.CellHeight - 300),
						this.tableView.Frame.Width,
						this.tableView.Frame.Height
					);

					this.tableView.ContentOffset = new CGPoint(0, this.configuration.BounceOffset);
				},
				completion: () => {
					this.tableView.RemoveFromSuperview();
					this.backgroundView.RemoveFromSuperview();
				}
			);
		}

		private void RotateArrow() {
			UIView.Animate(
				this.configuration.AnimationDuration,
				animation: () => {
					this.menuArrow.Transform = CGAffineTransform.Rotate(this.menuArrow.Transform, (nfloat)(Math.PI));
				});
		}

	}

	// MARK: BTConfiguration

	public class DropdownMenuTableView : UITableView, IUITableViewDelegate {
		public event EventHandler<ItemSelectedEventArgs> ItemSelected;

		public DropdownMenuTableView (CGRect frame, IEnumerable<string> items, DropdownMenuConfiguration configuration) 
			: base(frame, UITableViewStyle.Plain)
		{
			var source = new DropwdownMenuTableViewSource(items, configuration);

			source.ItemSelected += (sender, e) => {
				if (this.ItemSelected != null) {
					this.ItemSelected(this, e);
				}
			};

			this.Source = source;
			this.BackgroundColor = UIColor.Clear;
			this.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			this.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			this.TableFooterView = new UIView (CGRect.Empty);


		}

	}

	public class ItemSelectedEventArgs : EventArgs{
		public int Row {
			get;
			set;
		}

		public int Section {
			get;
			set;
		}

		public object Item {
			get;
			set;
		}
	}
	public class DropwdownMenuTableViewSource : UITableViewSource {
		public event EventHandler<ItemSelectedEventArgs> ItemSelected;


		public DropwdownMenuTableViewSource (IEnumerable<string> items, DropdownMenuConfiguration configuration)
		{
			this.items = items;
			this.selectedItemIndex = 0;
			this.configuration = configuration;
		}

		private DropdownMenuConfiguration configuration;
		private IEnumerable<string> items;
		private nint selectedItemIndex;

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableView, nint section)
		{
			return this.items.Count ();
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("BTTableViewCell") as DropdownMenuTableViewCell;

			if (cell == null) {
				cell = new DropdownMenuTableViewCell (UITableViewCellStyle.Default, "BTTableViewCell", this.configuration);
			}

			cell.TextLabel.Text = this.items.ElementAt (indexPath.Row) as string;
			cell.ContentView.BackgroundColor = this.configuration.CellBackgroundColor;

			cell.CheckMarkIcon.Hidden = (indexPath.Row != selectedItemIndex);

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return this.configuration.CellHeight;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			selectedItemIndex = indexPath.Row;

			if (ItemSelected != null) {
				ItemSelected (this, new ItemSelectedEventArgs {
					Section = indexPath.Section,
					Row = indexPath.Row,
					Item = items.ElementAt(indexPath.Row)
				});
			}

			tableView.ReloadData();
			var cell = tableView.CellAt (indexPath) as DropdownMenuTableViewCell;
			if (cell != null) {
				cell.ContentView.BackgroundColor = this.configuration.CellSelectionColor;
			}	
		}

		public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.CellAt (indexPath) as DropdownMenuTableViewCell;

			if (cell != null) {
				cell.CheckMarkIcon.Hidden = true;
				cell.ContentView.BackgroundColor = this.configuration.CellBackgroundColor;
			}
		}
	}

	// MARK: Table view cell
	public class DropdownMenuTableViewCell: UITableViewCell {
		public UIImageView CheckMarkIcon {
			get;
			set;
		}

		public CGRect CellContentFrame {
			get;
			set;
		}

		public DropdownMenuConfiguration Configuration {
			get;
			set;
		}

		public DropdownMenuTableViewCell (UITableViewCellStyle style, string reuseIdentifier, DropdownMenuConfiguration configuration)
			: base(style, reuseIdentifier)
		{
			this.Configuration = configuration;

			// Setup cell
			CellContentFrame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, this.Configuration.CellHeight);
			this.ContentView.BackgroundColor = this.Configuration.CellBackgroundColor;
			this.SelectionStyle = UITableViewCellSelectionStyle.None;
			this.TextLabel.TextAlignment = UITextAlignment.Left;
			this.TextLabel.TextColor = this.Configuration.CellTextLabelColor;
			this.TextLabel.Font = this.Configuration.CellTextLabelFont;
			this.TextLabel.Frame = new CGRect(20, 0, CellContentFrame.Width, CellContentFrame.Height);


			// Checkmark icon
			this.CheckMarkIcon = new UIImageView(new CGRect(CellContentFrame.Width - 50, (CellContentFrame.Height - 30)/2, 30, 30));
			this.CheckMarkIcon.Hidden = true;
			this.CheckMarkIcon.Image = this.Configuration.CheckMarkImage;
			this.CheckMarkIcon.ContentMode = UIViewContentMode.ScaleAspectFill;
			this.ContentView.AddSubview (this.CheckMarkIcon);

			// Separator for cell
			var separator = new DropdownMenuTableCellContentView(CellContentFrame);
			separator.BackgroundColor = UIColor.Clear;
			this.ContentView.AddSubview (separator);
		}

		public override void LayoutSubviews ()
		{
			this.Bounds = CellContentFrame;
			this.ContentView.Frame = this.Bounds;
		}
	}

	// Content view of table view cell
	public class DropdownMenuTableCellContentView: UIView {
		public DropdownMenuTableCellContentView(CGRect frame) : base(frame){
		}

		public override void Draw (CGRect rect)
		{
			base.Draw (rect);

			var context = UIGraphics.GetCurrentContext ();

			context.SetStrokeColor (0, 0, 0, (nfloat)0.5);
			context.SetLineWidth (1);
			context.MoveTo (0, this.Bounds.Size.Height);
			context.AddLineToPoint (this.Bounds.Size.Width, this.Bounds.Size.Height);
			context.StrokePath ();
		}
	}
}

