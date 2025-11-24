using System.Collections;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _shakeStrength = 0.2f;
    [SerializeField] private float _shakeLength = 0.25f;
    public static CameraController instance;
    private Coroutine _shakeCR;
    private Vector3 _originalPos;
    private bool _godmode;
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
        _shakeCR = null;

        EventManager.Subscribe(EventType.Miss, Shake);
        EventManager.Subscribe(EventType.Death, Unsub);
        EventManager.Subscribe(EventType.End, Unsub);
        RemoteConfigService.Instance.FetchCompleted += GodModeCheck;
    }

    private void GodModeCheck(ConfigResponse configResponse)
    {
        _godmode = RemoteConfigService.Instance.appConfig.GetBool("DevMode");
        if (_godmode)
        {
            EventManager.Unsubscribe(EventType.Miss, Shake);
        }
    }
    public void Shake(params object[] paramContainer)
    {
        if (_shakeCR != null)
        {
            StopCoroutine(_shakeCR);
        }
        _shakeCR = StartCoroutine(DoShake(_shakeStrength, _shakeLength));
    }

    private void Unsub(params object[] paramContainer)
    {
        EventManager.Unsubscribe(EventType.Miss, Shake);
        EventManager.Unsubscribe(EventType.Death, Unsub);
        EventManager.Unsubscribe(EventType.End, Unsub);
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
