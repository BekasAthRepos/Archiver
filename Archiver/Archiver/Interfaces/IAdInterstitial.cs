using System;
using System.Collections.Generic;
using System.Text;

namespace Archiver.Interfaces
{
    public interface IAdInterstitial
    {
        void LoadAd(string adUnitId);
        void ShowAd();
    }
}
