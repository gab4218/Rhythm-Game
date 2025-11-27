using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _optionsName = "Options";
    [SerializeField] private string _shopName = "Shop";
    public void Options() => ScreenManager.instance.Push(_optionsName);

    public void Shop() => ScreenManager.instance.Push(_shopName);
}
