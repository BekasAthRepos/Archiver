using Archiver.Model;
using MarcTron.Plugin;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace Archiver.Service
{
    public class AdManager
    {
        const int ADINTERCOUNT = 4;
        const int ADREWCOUNT = 9;
        private readonly ResourceManager _res;
        private string interId;
        private string rewardId;

        public AdManager() 
        {
            _res = new ResourceManager("Archiver.Resources.Strings", this.GetType().Assembly);
            interId = _res.GetString("AdInterstitialId").ToString();
            rewardId = _res.GetString("AdRewardedId").ToString();

            CrossMTAdmob.Current.LoadInterstitial(interId);
            CrossMTAdmob.Current.LoadRewarded(rewardId);
        }

        public async void ShowInterstitial()
        {
            SysIni ini = await App.Database.GetSysIniAsync("AdInterCount");
            int adCount = Int32.Parse(ini.Value);
            if (adCount >= ADINTERCOUNT)
            {
                CrossMTAdmob.Current.ShowInterstitial();
                CrossMTAdmob.Current.LoadInterstitial(interId);
                adCount = 0;
            }
            else
            {
                adCount++;
            }
            await App.Database.UpdateSysIniAsync(new SysIni { Code = "AdInterCount", Value = adCount.ToString() });
        }

        public async void ShowRewarded()
        {
            /*
            SysIni ini = await App.Database.GetSysIniAsync("AdRewardCount");
            int adCount = Int32.Parse(ini.Value);
            if (adCount >= ADREWCOUNT)
            {
                CrossMTAdmob.Current.ShowRewarded();
                CrossMTAdmob.Current.LoadRewarded(rewardId);
                adCount = 0;
            }
            else
            {
                adCount++;
            }
            await App.Database.UpdateSysIniAsync(new SysIni { Code = "AdRewardCount", Value = adCount.ToString() });
            */
        }
    }
}
