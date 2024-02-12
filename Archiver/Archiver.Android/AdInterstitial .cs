/*
using Android.Gms.Ads;
using Xamarin.Forms;
using Archiver.Droid;
using Archiver.Interfaces;
using Android.Gms.Ads.Interstitial;

[assembly: Dependency(typeof(AdInterstitial))]
namespace Archiver.Droid
{
    public class AdInterstitial : IAdInterstitial
    {
        InterstitialAd interstitialAd;

        public void LoadAd(string adUnitId)
        {
            interstitialAd = new InterstitialAd(Application.Context);
            interstitialAd.AdUnitId = adUnitId;

            interstitialAd.AdListener = new AdListener();
            interstitialAd.AdLoaded += (sender, args) =>
            {
                // Ad loaded callback, if needed
            };

            AdRequest adRequest = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(adRequest);
        }

        public void ShowAd()
        {
            if (interstitialAd != null && interstitialAd.IsLoaded)
            {
                interstitialAd.Show();
            }
        }

    }
} 
*/