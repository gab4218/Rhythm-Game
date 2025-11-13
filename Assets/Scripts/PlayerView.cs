using UnityEngine;

public class PlayerView
{
    private Animator _anim;

    public PlayerView(Animator anim)
    {
        _anim = anim;
        EventManager.Subscribe(EventType.Hit, Hit);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.Death, End);
        EventManager.Subscribe(EventType.End, End);
    }

    public void Hit(params object[] obj)
    {
        _anim.SetTrigger("Hit");
    }

    public void Miss(params object[] obj)
    {
        _anim.SetTrigger("Miss");
    }

    public void End(params object[] obj)
    {
        EventManager.Unsubscribe(EventType.Hit, Hit);
        EventManager.Unsubscribe(EventType.Miss, Miss);
        EventManager.Unsubscribe(EventType.Death, End);
        EventManager.Unsubscribe(EventType.End, End);
    }
}
