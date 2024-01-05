using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Webkit;
using AndroidX.AppCompat.App;
using Android.Content;
using Android.Views;

namespace XamarinAndroid.MyTube
{
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
        DataSchemes = new[] { "http", "https" },
        DataHosts = new[] { "youtu.be", "youtube.com", "m.youtube.com", "www.youtube.com" },
        AutoVerify = true)]
    [Activity(Label = "@string/app_name",
        Theme = "@style/Theme.AppCompat.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
        MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private WebView webView;
        private MyWebChromeClient chromeClient;
        private const string homeUrl = "https://m.youtube.com/";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            webView = (WebView)FindViewById(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.ForceDark = ForceDarkMode.On;
            webView.SetWebViewClient(new MyWebViewClient(Assets));
            chromeClient = new MyWebChromeClient(Window.DecorView, this);
            webView.SetWebChromeClient(chromeClient);

            if (string.IsNullOrEmpty(Intent.DataString))
            {
                if (savedInstanceState == null)
                    webView.LoadUrl(homeUrl);
                else
                    webView.RestoreState(savedInstanceState);
            }
            else
                webView.LoadUrl(Intent.DataString.Replace("www", "m"));
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus && chromeClient.IsFullScreen())
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.HideNavigation |
                                                                            SystemUiFlags.Fullscreen |
                                                                            SystemUiFlags.ImmersiveSticky);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            webView.SaveState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            webView.RestoreState(savedInstanceState);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                if (chromeClient.OnBackPressed())
                    return true;

                if (webView.CanGoBack() && webView.Url != homeUrl)
                {
                    webView.GoBack();
                    return true;
                }
            }

            return base.OnKeyDown(keyCode, e);
        }
    }
}