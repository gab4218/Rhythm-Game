using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string _id = "Rewarded_Android";
    public void Initialize(string id)
    {
        _id = id;
        LoadAd();
    }

    public void LoadAd() => Advertisement.Load(_id, this);
    public void ShowAd() => Advertisement.Show(_id, this);

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == _id)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) StaminaManager.instance.AddStamina(5);
            else if (showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED)) StaminaManager.instance.AddStamina(1);
            else if (showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN)) Debug.Log("Huh");
        }
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log(message);
    }

    public void OnUnityAdsShowStart(string placementId)
    { }

}
