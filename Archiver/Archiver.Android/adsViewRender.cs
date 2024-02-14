using Android.Content;
using Android.Gms.Ads;
using Archiver.Ads;
using Archiver.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(adsView), typeof(adsViewRender))]
namespace Archiver.Droid
{
    public class adsViewRender:ViewRenderer<adsView, AdView>
    {
        private string bannerId = "ca-app-pub-3940256099942544/6300978111"; // testId = "ca-app-pub-3940256099942544/6300978111"
                                                                            // prodId = "ca-app-pub-5254772256939632/1295811312"

        public adsViewRender(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<adsView> e)
        {
            base.OnElementChanged(e);
            if(Control ==  null)
            {
                var adview = new AdView(Context)
                {
                    AdSize = AdSize.Banner,
                    AdUnitId = bannerId
                };
                adview.LoadAd(new AdRequest.Builder().Build());
                SetNativeControl(adview);
            }
        }
    }
}