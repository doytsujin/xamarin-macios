// Copyright 2011-2012 Xamarin Inc. All rights reserved

#if !__TVOS__ && !__WATCHOS__ && !MONOMAC

using System;
using System.Drawing;
using System.Reflection;
#if XAMCORE_2_0
using Foundation;
using UIKit;
#else
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using NUnit.Framework;

#if XAMCORE_2_0
using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using PointF=CoreGraphics.CGPoint;
#else
using nfloat=global::System.Single;
using nint=global::System.Int32;
using nuint=global::System.UInt32;
#endif

namespace MonoTouchFixtures.UIKit {
	
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ActionSheetTest {
		
		void CheckDefault (UIActionSheet a)
		{
			Assert.That (a.ButtonCount, Is.EqualTo ((nint) 0), "ButtonCount");
			Assert.That (a.CancelButtonIndex, Is.EqualTo ((nint) (-1)), "CancelButtonIndex");
			Assert.Null (a.Delegate, "Delegate");
			Assert.That (a.DestructiveButtonIndex, Is.EqualTo ((nint) (-1)), "DestructiveButtonIndex");
			Assert.That (a.FirstOtherButtonIndex, Is.EqualTo ((nint) (-1)), "FirstOtherButtonIndex");

			var style = TestRuntime.CheckiOSSystemVersion (8, 0) ? UIActionSheetStyle.Default : UIActionSheetStyle.Automatic;
			Assert.That (a.Style, Is.EqualTo (style), "Style");

			Assert.Null (a.Title, "Title");

			Assert.False (a.Visible, "Visible");

			Assert.Null (a.WeakDelegate, "WeakDelegate");
		}
		
		[Test]
		public void CtorDefault ()
		{
			using (UIActionSheet a = new UIActionSheet ()) {
				CheckDefault (a);
			}
		}

		[Test]
		public void CtorAllNulls ()
		{
			// http://bugzilla.xamarin.com/show_bug.cgi?id=3081
			using (UIActionSheet a = new UIActionSheet (null, null, null, null)) {
				CheckDefault (a);
			}
		}

		[Test]
		public void InitWithFrame ()
		{
			RectangleF frame = new RectangleF (10, 10, 100, 100);
			using (UIActionSheet a = new UIActionSheet (frame)) {
				Assert.That (a.Frame, Is.EqualTo (frame), "Frame");
				CheckDefault (a);
			}
		}

		class MyActionSheetDelegate : UIActionSheetDelegate {
		}
		
		[Test]
		public void CtorDelegate ()
		{
			using (var del = new MyActionSheetDelegate ())
			using (var a = new UIActionSheet ("title", del, null, null, null)) {
				Assert.That (a.Title, Is.EqualTo ("title"), "Title");
				Assert.NotNull (typeof (UIActionSheet).GetField ("__mt_WeakDelegate_var", BindingFlags.Instance | BindingFlags.NonPublic).GetValue (a), "backing field");
				// check properties after the field (so we're not setting it only when calling the properties)
				Assert.NotNull (a.Delegate, "Delegate");
				Assert.NotNull (a.WeakDelegate, "WeakDelegate");
			}
		}
	}
}

#endif // !__TVOS__ && !__WATCHOS__
