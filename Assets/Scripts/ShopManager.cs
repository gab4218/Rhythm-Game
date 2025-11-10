using System;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int defaultPrice = 500;

    public static ShopManager instance;

    public event Action OnPriceChange;

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
    }

    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += ChangePrice;
    }

    private void ChangePrice(ConfigResponse configResponse)
    {
        defaultPrice = RemoteConfigService.Instance.appConfig.GetInt("BaseSkinCost");
        OnPriceChange?.Invoke();
    }
    
}
