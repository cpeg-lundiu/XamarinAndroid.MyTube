using Android.Content.PM;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace XamarinAndroid.MyTube
{
    internal class MyWebChromeClient : WebChromeClient
    {
        private View customView;
        private readonly View decorView;
        private readonly MainActivity mainActivity;
        private ICustomViewCallback customViewCallback;
        private ScreenOrientation originalOrientation;
        private bool isFullScreen;

        public MyWebChromeClient(View pDecorView, MainActivity pMainActivity)
        {
            decorView = pDecorView;
            mainActivity = pMainActivity;
            isFullScreen = false;
            originalOrientation = mainActivity.RequestedOrientation;
        }

        public override void OnHideCustomView()
        {
            ((FrameLayout)decorView).RemoveView(customView);
            customView = null;
            mainActivity.RequestedOrientation = originalOrientation;
            decorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
            customViewCallback.OnCustomViewHidden();
            customViewCallback = null;
            isFullScreen = false;
            mainActivity.FindViewById<WebView>(Resource.Id.webView).ClearFocus();
        }

        public override void OnShowCustomView(View pView, ICustomViewCallback pCustomViewCallback)
        {
            if (customView != null)
            {
                OnHideCustomView();
                return;
            }

            customView = pView;
            customViewCallback = pCustomViewCallback;
            ((FrameLayout)decorView).AddView(customView, new FrameLayout.LayoutParams(-1, -1));
            isFullScreen = true;
            originalOrientation = mainActivity.RequestedOrientation;
            mainActivity.RequestedOrientation = ScreenOrientation.Landscape;
            decorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.HideNavigation |
                                                                 SystemUiFlags.Fullscreen |
                                                                 SystemUiFlags.ImmersiveSticky);
        }

        public bool OnBackPressed()
        {
            if (customView != null)
            {
                OnHideCustomView();
                return true;
            }
            else
                return false;
        }

        public bool IsFullScreen()
        {
            return isFullScreen;
        }
    }
}