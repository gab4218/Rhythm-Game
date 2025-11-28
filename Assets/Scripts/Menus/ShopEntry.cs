using TMPro;
using UnityEngine;

public class ShopEntry : MonoBehaviour
{
    [SerializeField] private float _priceMult = 1;
    [SerializeField] private TMP_Text _priceText;
    public int price;
    public int item;
    private void Start()
    {
        if (InventoryManager.unlockedCosmetics == default || InventoryManager.unlockedCosmetics.Length <= 0) InventoryManager.unlockedCosmetics = new bool[3] { true, false, false };
        if (InventoryManager.unlockedCosmetics[item])
        {
            Destroy(_priceText.gameObject);
            Destroy(this);
        }
        price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        ShopManager.instance.OnPriceChange += ChangePrice;
        _priceText.text = "Price: " + price;
    }

    private void ChangePrice()
    {
        price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        _priceText.text = "Price: " + price;
    }

    public void Buy()
    {
        MoneyManager.instance.Purchase(this);
        if (InventoryManager.unlockedCosmetics[item])
        {
            Destroy(_priceText.gameObject);
            Destroy(this);
        }
    }
}
