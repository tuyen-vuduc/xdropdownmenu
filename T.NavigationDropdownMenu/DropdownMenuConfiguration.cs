using System;
using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using System.Linq;

namespace T.NavigationDropdownMenu
{

	// MARK: BTConfiguration
	public class DropdownMenuConfiguration {
		public nfloat CellHeight {
			get;
			set;
		}

		public UIColor CellBackgroundColor {
			get;
			set;
		}
		public UIColor CellTextLabelColor {
			get;
			set;
		}

		public UIFont CellTextLabelFont {
			get;
			set;
		}

		public UIColor CellSelectionColor {
			get;
			set;
		}

		public UIImage CheckMarkImage {
			get;
			set;
		}

		public UIImage ArrowImage {
			get;
			set;
		}

		public nfloat ArrowPadding {
			get;
			set;
		}

		public double AnimationDuration {
			get;
			set;
		}

		public UIColor MaskBackgroundColor {
			get;
			set;
		}

		public nfloat MaskBackgroundOpacity {
			get;
			set;
		}

		public nfloat BounceOffset {
			get;
			set;
		}

		public DropdownMenuConfiguration ()
		{
			this.Init ();
		}

		private void Init() {
			// Default values
			this.CellHeight = 50;
			this.CellBackgroundColor = UIColor.White;
			this.CellTextLabelColor = UIColor.DarkGray;
			this.CellTextLabelFont = UIFont.FromName ("HelveticaNeue-Bold", 17);
			this.CellSelectionColor = UIColor.LightGray;
			this.CheckMarkImage = UIImage.FromFile ("checkmark_icon.png");
			this.AnimationDuration = 0.3;
			this.BounceOffset = 10;
			this.ArrowImage = UIImage.FromFile("arrow_down_icon.png");
			this.ArrowPadding = 15;
			this.MaskBackgroundColor = UIColor.Black;
			this.MaskBackgroundOpacity = (nfloat)0.3;
		}
	}

	// MARK: Table view cell

	// Content view of table view cell
	
}
