// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace BeaconTouch
{
	[Register ("InfoViewController")]
	partial class InfoViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView FullView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel InfoLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (InfoLabel != null) {
				InfoLabel.Dispose ();
				InfoLabel = null;
			}

			if (FullView != null) {
				FullView.Dispose ();
				FullView = null;
			}
		}
	}
}
