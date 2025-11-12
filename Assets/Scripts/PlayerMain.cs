using Unity.Services.RemoteConfig;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerModel _model;
    private IController _controller;
    private bool _inverted = false;
    [SerializeField] private Transform[] _lanes;
    [SerializeField] private Animator _anim;
    [SerializeField] private UnityEngine.UI.Image _hpImage;
    [SerializeField] private float _noteDetectionRange = 2f;
    [SerializeField] private float _health = 1f;
    [SerializeField] private float _lerpSpeed = 10f;
    private float _mobileThreshold;

    private void Start()
    {
        _model = new PlayerModel(transform, _lanes, _hpImage).HP(_health).Range(_noteDetectionRange).LerpSpeed(_lerpSpeed);

        _mobileThreshold = Screen.width/2;
#if UNITY_STANDALONE_WIN
        _controller = new PlayerControllerPC(_model);
        
#elif UNITY_ANDROID
        _controller = new PlayerControllerMobile(_model).SetBounds(_mobileThreshold).SetInverted(_inverted);
#endif
        GameManager.instance.player = _model;
        RemoteConfigService.Instance.FetchCompleted += CheckInverted;
        EventManager.Subscribe(EventType.Death, Death);
        EventManager.Subscribe(EventType.End, Death);
    }

    private void Update()
    {
        if (_controller == null) return;

        _controller.OnUpdate();
        _model.OnUpdate();
    }

    private void CheckInverted(ConfigResponse configResponse)
    {
        _inverted = RemoteConfigService.Instance.appConfig.GetBool("MirroredMobileControls");
    }

    private void Death(params object[] paramContainer)
    {
        _controller = null;
        EventManager.Unsubscribe(EventType.Death, Death);
        EventManager.Unsubscribe(EventType.End, Death);
    }
}
