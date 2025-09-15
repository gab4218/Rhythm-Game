using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerModel _model;
    private IController _controller;
    [SerializeField] private Transform[] _lanes;
    

    private void Start()
    {
        _model = new PlayerModel(transform, _lanes);
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
