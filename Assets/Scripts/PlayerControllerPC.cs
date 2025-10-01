using UnityEngine;

public class PlayerControllerPC : IController
{
    private PlayerModel _model;
    private bool _holding = false;

    public PlayerControllerPC(PlayerModel model)
    {
        _model = model;
    }

    public void OnUpdate()
    {
        if (_model == null) return;

        

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) _model.SwitchLane(Input.GetAxisRaw("Horizontal"));
        
        if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.J)) && _holding)
        {
            _model.ReleaseHit();
            _holding = false;
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J)) && !_holding)
        {
            _model.PressHit();
            _holding = true;
        }

        if (_holding)
        {
            _model.HoldHit();
        }
    }
}
