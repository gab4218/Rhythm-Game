using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _endGroup, _normalGroup;

    void Start()
    {
        _endGroup.alpha = 0;
        _endGroup.blocksRaycasts = false;
        EventManager.Subscribe(EventType.End, End);
        EventManager.Subscribe(EventType.Death, End);
    }

    private void End(params object[] paramContainer)
    {
        StartCoroutine(EndCR());
        EventManager.Unsubscribe(EventType.End, End);
        EventManager.Unsubscribe(EventType.Death, End);
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private IEnumerator EndCR()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        t = 0;
        while (t < 2f)
        {
            _endGroup.alpha = t / 2f;
            _normalGroup.alpha = 1f - t / 2f;
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        _normalGroup.alpha = 0;
        _endGroup.alpha = 1;
        _endGroup.blocksRaycasts = true;
    }

}
