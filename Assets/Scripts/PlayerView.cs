using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{
    private Animator _anim;

    public PlayerView(Animator anim)
    {
        _anim = anim;
        EventManager.Subscribe(EventType.Hit, Hit);
    }

    public void Hit(params object[] obj)
    {
        _anim.SetTrigger("Hit");
    }
}
