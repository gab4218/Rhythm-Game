using Unity.Services.RemoteConfig;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerModel _model;
    private IController _controller;
    private bool _inverted = false;
    [SerializeField] private Transform[] _lanes;
    [SerializeField] private Transform _chartParent;
    [SerializeField] private Animator _anim;
    [SerializeField] private Material _mat;
    [SerializeField] private Material _dmgMat;
    [SerializeField] private Material _deathMat;
    [SerializeField] private Material _skinMat;
    [SerializeField] private UnityEngine.UI.Image _hpImage;
    [SerializeField] private float _noteDetectionRange = 2f;
    [SerializeField] private float _health = 1f;
    [SerializeField] private float _lerpSpeed = 10f;
    private float _mobileThreshold;
    [SerializeField] private Terrain _terrain;

    private void Start()
    {
        _model = new PlayerModel(transform, _chartParent, _lanes, _hpImage).HP(_health).Range(_noteDetectionRange).LerpSpeed(_lerpSpeed);
        _dmgMat.SetInt("_Enabled", 0);
        _deathMat.SetInt("_Enabled", 0);
        _mobileThreshold = Screen.width/2;
#if UNITY_STANDALONE_WIN
        _controller = new PlayerControllerPC(_model, new PlayerView(_anim, _model, this).SetMat(_mat).SetDeathMat(_deathMat).SetDmgMat(_dmgMat).SetSkinMat(_skinMat));

#elif UNITY_ANDROID
        _controller = new PlayerControllerMobile(_model, new PlayerView(_anim, _model, this).SetMat(_mat).SetDeathMat(_deathMat).SetDmgMat(_dmgMat).SetSkinMat(_skinMat)).SetBounds(_mobileThreshold).SetInverted(_inverted);
#endif
        GameManager.instance.player = _model;
        RemoteConfigService.Instance.FetchCompleted += CheckInverted;
        EventManager.Subscribe(EventType.Death, Death);
        EventManager.Subscribe(EventType.End, Death);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenManager.instance.Push("Pause");
        }
        if (_controller == null) return;

        _controller.OnUpdate();
        _model.OnUpdate();
        _terrain.terrainData.terrainLayers[0].tileOffset += new Vector2(0, 10f * Time.deltaTime);
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

    private void OnDestroy()
    {
        _dmgMat.SetInt("_Enabled", 0);
        _deathMat.SetInt("_Enabled", 0);
    }
}
