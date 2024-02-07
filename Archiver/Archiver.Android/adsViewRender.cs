using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Archiver.Ads;
using Archiver.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(adsView), typeof(adsViewRender))]
namespace Archiver.Droid
{
    public class adsViewRender:ViewRenderer<adsView, AdView>
    {
        public adsViewRender(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<adsView> e)
        {
            base.OnElementChanged(e);
            if(Control ==  null)
            {
                var adview = new AdView(Context)
                {
                    AdSize = AdSize.Banner, //or newAdSize(300,60)
                    AdUnitId = "ca-app-pub-3940256099942544/6300978111"
                };

                adview.LoadAd(new AdRequest.Builder().Build());
                SetNativeControl(adview);
            }
        }
    }
}