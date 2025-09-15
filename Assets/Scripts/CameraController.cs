using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private Coroutine _shakeCR;
    private Vector3 _originalPos;

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
        _originalPos = transform.localPosition;
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.Miss, Shake);
    }

    public void Shake()
    {
        if (_shakeCR != null)
        {
            StopCoroutine(_shakeCR);
        }
        _shakeCR = StartCoroutine(DoShake(0.25f, 0.2f));
    }

    private IEnumerator DoShake(float intensity, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * intensity * 1f;
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _originalPos;
    }
}
