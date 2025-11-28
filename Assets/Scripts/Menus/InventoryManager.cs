using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static bool[] unlockedCosmetics = new bool[]{true, false, false};

    [SerializeField] private GameObject[] _unlockImages;

    public static InventoryManager instance;

    public static Color selectedColor = Color.black;

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
        if (unlockedCosmetics == default) unlockedCosmetics = new bool[3];
        for (int i = 0; i < unlockedCosmetics.Length; i++)
        {
            _unlockImages[i].SetActive(!unlockedCosmetics[i]);
        }
    }


    public void SelectColor(Image col)
    {
        selectedColor = col.color;
    }
    
}
