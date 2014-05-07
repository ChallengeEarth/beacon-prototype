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
	[Register ("BeaconTouchViewController")]
	partial class BeaconTouchViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel infoMessage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView view { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (view != null) {
				view.Dispose ();
				view = null;
			}

			if (infoMessage != null) {
				infoMessage.Dispose ();
				infoMessage = null;
			}
		}
	}
}
