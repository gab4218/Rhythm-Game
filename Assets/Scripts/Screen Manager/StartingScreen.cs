using UnityEngine;

public class StartingScreen : MonoBehaviour
{
    [SerializeField] private ScreenGO _startingScreen;

    private void Start()
    {
        ScreenManager.instance.Push(_startingScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseScreen.paused)
            {
                ScreenManager.instance.Clear();
            }
        }
    }

}
