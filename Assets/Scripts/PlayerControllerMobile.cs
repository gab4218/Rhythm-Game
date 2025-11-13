using UnityEngine;

public class PlayerControllerMobile : IController
{
    private PlayerModel _model = default;
    private PlayerView _view;
    private bool _holding = false;
    private bool _inverted = false;
    private float _changeThreshold = 500f;
    private Touch _rightTouch;
    private Touch _leftTouch;
    private SwipeData _swipeData;


    public PlayerControllerMobile(PlayerModel model, PlayerView view)
    {
        _model = model;
        _view = view;
        _swipeData = new SwipeData();
    }

    public PlayerControllerMobile SetBounds(float threshold)
    {
        _changeThreshold = threshold;
        return this;
    }

    public PlayerControllerMobile SetInverted(bool inverted)
    {
        _inverted = inverted;
        return this;
    }

    public void OnUpdate()
    {
        if (_model == null) return;
        if (Input.touchCount == 0) return;
        bool foundRight = false;
        bool foundLeft = false;

        foreach (Touch t in Input.touches)
        {

            if (_inverted)
            {
                if (t.position.x < _changeThreshold)
                {
                    _rightTouch = t;
                    foundRight = true;
                }
                else
                {
                    _leftTouch = t;
                    foundLeft = true;
                }
            }
            else
            {
                if (t.position.x > _changeThreshold)
                {
                    _rightTouch = t;
                    foundRight = true;
                }
                else
                {
                    _leftTouch = t;
                    foundLeft = true;
                }

            }
        }
        Debug.Log(_leftTouch.position.x);

        if(foundRight)
        {
            if (_rightTouch.phase == TouchPhase.Began)
            {
                _model.PressHit();
                _holding = true;
            } 
            else if (_rightTouch.phase == TouchPhase.Ended || _rightTouch.phase == TouchPhase.Canceled)
            {
                _model.ReleaseHit();
                _holding = false;
            }
            if (_holding)
            {
                _model.HoldHit();
            }
        }
        if (foundLeft)
        {
            if (_leftTouch.phase == TouchPhase.Began)
            {
                _swipeData.StartSwipe(_leftTouch.position);
            }
            else if (_leftTouch.phase == TouchPhase.Ended || _leftTouch.phase == TouchPhase.Canceled)
            {
                _swipeData.EndSwipe(_leftTouch.position);
                _model.SwitchLane(-_swipeData.direction.x);
            }
        }

    }

}
