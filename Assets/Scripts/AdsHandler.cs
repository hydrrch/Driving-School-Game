using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsHandler : MonoBehaviour
{

    public static AdsHandler instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }


    public bool IsInternetAvailable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) return true;
        else return false;
    }

    public void ShowInterstitial()
    {
        if (GoogleAdMobControllerM.Instance)
            GoogleAdMobControllerM.Instance.ShowInterstitialAd();
    }

    public void ShowRec()
    {
        if (GoogleAdMobControllerM.Instance)
            GoogleAdMobControllerM.Instance.ShowRectBanner();
    }

    public void HideRec()
    {
        if (GoogleAdMobControllerM.Instance)
            GoogleAdMobControllerM.Instance.HideRectBanner();
    }

    public void ShowRewarded()
    {
        if (GoogleAdMobControllerM.Instance)
            GoogleAdMobControllerM.Instance.ShowRewardedAd();
    }
}
