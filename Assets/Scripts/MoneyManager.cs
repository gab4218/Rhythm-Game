using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public static int money = 0;
    [SerializeField] private TMP_Text _text;
    private void Start()
    {
        _text.text = money.ToString();
    }

    private void Update()
    {
        _text.text = money.ToString();
    }
}
