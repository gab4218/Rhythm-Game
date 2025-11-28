using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int defaultPrice = 500;

    public static ShopManager instance;

    public event Action OnPriceChange;

    public List<TMP_Text> text;

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
        MoneyManager.instance.text.AddRange(text);
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
    
    public void Inventory()
    {
        ScreenManager.instance.Push("Inventory");
    }
}
