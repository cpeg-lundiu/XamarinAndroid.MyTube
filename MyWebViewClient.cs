using Android.Graphics;
using Android.Webkit;
using Android.Content.Res;
using System.IO;

namespace XamarinAndroid.MyTube
{
    internal class MyWebViewClient : WebViewClient
    {
        private readonly AssetManager assetManager;

        public MyWebViewClient(AssetManager pAssetManager)
        {
            assetManager = pAssetManager;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return false;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            string content;

            using (StreamReader sr = new StreamReader(assetManager.Open("magic.js")))
            {
                content = sr.ReadToEnd();
            }

            view.EvaluateJavascript(content, null);
            base.OnPageStarted(view, url, favicon);
        }
    }
}