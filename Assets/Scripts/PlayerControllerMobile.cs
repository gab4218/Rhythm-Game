using UnityEngine;

public class PlayerControllerMobile : IController
{
    private PlayerModel _model = default;
    private bool _holding = false;
    private bool _inverted = false;
    private float _changeThreshold = 500f;
    private Touch _rightTouch;
    private Touch _leftTouch;
    private SwipeData _swipeData;


    public PlayerControllerMobile(PlayerModel model)
    {
        _model = model;
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


        foreach (Touch t in Input.touches)
        {
            if (t.position.x > _changeThreshold)
            {
                _rightTouch = t;
            }
            else
            {
                _leftTouch = t;
            }
        }
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
        if (_leftTouch.phase == TouchPhase.Began)
        {
            _swipeData.StartSwipe(_leftTouch.position);
        }
        else if (_leftTouch.phase == TouchPhase.Ended || _leftTouch.phase == TouchPhase.Canceled)
        {
            _swipeData.EndSwipe(_leftTouch.position);
            _model.SwitchLane(_swipeData.direction.x);
        }

    }

}
