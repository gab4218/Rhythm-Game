using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerModel _model;
    private IController _controller;
    [SerializeField] private Transform[] _lanes;
    [SerializeField] private UnityEngine.UI.Image _hpImage;
    [SerializeField] private float _noteDetectionRange = 2f;
    [SerializeField] private float _health = 1f;
    [SerializeField] private float _lerpSpeed = 10f;

    private void Start()
    {
        _model = new PlayerModel(transform, _lanes, _hpImage).HP(_health).Range(_noteDetectionRange).LerpSpeed(_lerpSpeed);
        _controller = new PlayerControllerPC(_model);
        GameManager.instance.player = _model;
        EventManager.Subscribe(EventType.Death, Death);
        EventManager.Subscribe(EventType.End, Death);
    }

    private void Update()
    {
        if (_controller == null) return;

        _controller.OnUpdate();
        _model.OnUpdate();
    }

    private void Death(params object[] paramContainer)
    {
        _controller = null;
        EventManager.Unsubscribe(EventType.Death, Death);
        EventManager.Unsubscribe(EventType.End, Death);
    }
}
