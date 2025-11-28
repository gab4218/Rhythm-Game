using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public static int money;
    public List<TMP_Text> text;
    [SerializeField] private AudioClip _goodClip, _badClip;

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

    private void Start()
    {
        foreach (var t in text)
        {
            t.text = money.ToString();

        }
    }

    private void Update()
    {
        foreach (var t in text)
        {
            t.text = money.ToString();

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            money += 100;
        }
    }

    public void Purchase(ShopEntry entry)
    {
        if (entry.price > money)
        {
            SoundSingleton.instance.sfxSource.PlayOneShot(_badClip);
            return;
        }
        money -= entry.price;
        Destroy(entry);
        InventoryManager.unlockedCosmetics[entry.item] = true;
        SoundSingleton.instance.sfxSource.PlayOneShot(_goodClip);
    }
}
