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
        price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        ShopManager.instance.OnPriceChange += ChangePrice;
        _priceText.text = "Price: " + price;
    }

    private void ChangePrice()
    {
        price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        _priceText.text = "Price: " + price;
    }

    public void Unlock()
    {

    }

}
