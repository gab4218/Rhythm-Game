using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerModel _model;
    private IController _controller;
    [SerializeField] private Transform[] _lanes;
    [SerializeField] private UnityEngine.UI.Image _hpImage;

    private void Start()
    {
        _model = new PlayerModel(transform, _lanes, _hpImage);
        _controller = new PlayerControllerPC(_model);
        GameManager.instance.player = _model;
    }

    private void Update()
    {
        if (_controller == null) return;

        _controller.OnUpdate();
        _model.OnUpdate();
    }
}
