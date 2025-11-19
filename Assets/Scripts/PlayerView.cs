using UnityEngine;

public class PlayerView
{
    private Animator _anim;

    private Material _material;

    private Color _startCol;

    private Color _startEm;

    private PlayerModel _model;

    public PlayerView(Animator anim, PlayerModel model)
    {
        _anim = anim;
        _model = model;
        EventManager.Subscribe(EventType.Hit, Hit);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.Death, End);
        EventManager.Subscribe(EventType.End, End);
    }

    public PlayerView SetMat(Material mat)
    {
        _material = mat;
        _material.SetColor("_EmissionColor", new Color(0, 5, 5));
        _material.color = new Color(0.5f, 0.5f, 0.5f);
        _startCol = mat.color;
        _startEm = mat.GetColor("_EmissionColor");
        return this;    
    }

    private void SetMatHealth()
    {
        _material.color = Color.Lerp(Color.red, _startCol, _model.health);
        _material.SetColor("_EmissionColor", Color.Lerp(Color.black, _startEm, _model.health) );
        if (_model.health <= 0)
        {
            _material.color = _startCol;
            _material.SetColor("_EmissionColor", _startEm);
        }
    }


    public void Hit(params object[] obj)
    {
        _anim.SetTrigger("Hit");
        SetMatHealth();
    }

    public void Miss(params object[] obj)
    {
        _anim.SetTrigger("Miss");
        SetMatHealth();
    }

    public void End(params object[] obj)
    {
        _material.color = _startCol;
        _material.SetColor("_EmissionColor", _startEm);
        EventManager.Unsubscribe(EventType.Hit, Hit);
        EventManager.Unsubscribe(EventType.Miss, Miss);
        EventManager.Unsubscribe(EventType.Death, End);
        EventManager.Unsubscribe(EventType.End, End);
    }
}
