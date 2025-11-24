using TMPro;
using UnityEngine;

public class ShopEntry : MonoBehaviour
{
    [SerializeField] private float _priceMult = 1;
    [SerializeField] private TMP_Text _priceText;
    private int _price;
    private void Start()
    {
        _price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        ShopManager.instance.OnPriceChange += ChangePrice;
        _priceText.text = "Price: " + _price;
    }

    private void ChangePrice()
    {
        _price = (int)(ShopManager.instance.defaultPrice * _priceMult);
        _priceText.text = "Price: " + _price;
    }

}
