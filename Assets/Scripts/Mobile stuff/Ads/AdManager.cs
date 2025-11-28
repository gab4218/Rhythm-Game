using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

[RequireComponent(typeof(RewardedAds))]
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{

    [SerializeField] private bool _testing = true;
    public static AdManager instance;
    private string _androidAdsId = "5993749";
    [SerializeField] private RewardedAds _rewardedAds;
    private string _androidRewardedAdsId = "Rewarded_Android";



    public void OnInitializationComplete()
    {
        if (_rewardedAds == null) _rewardedAds = GetComponent<RewardedAds>();
        _rewardedAds.Initialize(_androidRewardedAdsId);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void RewardedAd() => _rewardedAds.ShowAd();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_androidAdsId, _testing, this);
        }
    }
}
