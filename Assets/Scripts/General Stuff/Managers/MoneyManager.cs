using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public static int money = 0;
    [SerializeField] private TMP_Text[] _text;
    private void Start()
    {
        foreach (var t in _text)
        {
            t.text = money.ToString();

        }
    }

    private void Update()
    {
        foreach (var t in _text)
        {
            t.text = money.ToString();

        }
    }

    public void Purchase(ShopEntry entry)
    {
        if (entry.price > money) return;
        money -= entry.price;
        InventoryManager.instance.Unlock(entry.item);
    }
}
