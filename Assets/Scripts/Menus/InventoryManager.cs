using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static bool[] unlockedCosmetics;

    [SerializeField] private Image[] _unlockImages;

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
        if (unlockedCosmetics == default) unlockedCosmetics = new bool[_unlockImages.Length];

        for (int i = 0; i < _unlockImages.Length; i++)
        {
            _unlockImages[i].gameObject.SetActive(!unlockedCosmetics[i]);
        }
    }

    public void Unlock(int piece)
    {
        unlockedCosmetics[piece] = true;
        _unlockImages[piece].gameObject.SetActive(false);
    }

    public void SelectColor(Color col)
    {
        selectedColor = col;
    }
    
}
