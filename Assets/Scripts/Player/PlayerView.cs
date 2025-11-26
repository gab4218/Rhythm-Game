using System.Collections;
using UnityEngine;

public class PlayerView
{
    private Animator _anim;

    private Material _material;

    private Material _damageMaterial;

    private Material _deathMaterial;

    private Color _startCol;

    private Color _startEm;

    private PlayerModel _model;

    private PlayerMain _main;

    private Coroutine _dmgCR, _deathCR;

    public PlayerView(Animator anim, PlayerModel model, PlayerMain main)
    {
        _anim = anim;
        _model = model;
        _main = main;
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

    public PlayerView SetDmgMat(Material mat)
    {
        _damageMaterial = mat;
        return this;
    }

    public PlayerView SetDeathMat(Material mat)
    {
        _deathMaterial = mat;
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
        if (_model.health > 0)
        {
            if(_dmgCR != null) _main.StopCoroutine(_dmgCR);
            _dmgCR = _main.StartCoroutine(DamageEffect());
        }
        else
        {
            if (_dmgCR != null) _main.StopCoroutine(_dmgCR);
            _deathCR = _main.StartCoroutine(DeathEffect());
        }
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

    private IEnumerator DamageEffect()
    {
        float t = 0;
        _damageMaterial.SetInt("_Enabled", 1);
        _damageMaterial.SetFloat("_DistortionStrength", 1f);
        while (t < 1)
        {
            _damageMaterial.SetFloat("_DistortionStrength", 1f - t);
            t += Time.deltaTime * 4f;
            yield return null;
        }
        _damageMaterial.SetInt("_Enabled", 0);
        _dmgCR = null;
    }

    private IEnumerator DeathEffect()
    {
        float t = 0;
        _deathMaterial.SetInt("_Enabled", 1);
        _deathMaterial.SetFloat("_Progress", 0f);
        while (t < 1)
        {
            _deathMaterial.SetFloat("_Progress", t);
            t += Time.unscaledDeltaTime * 0.5f;
            yield return null;
        }
        _deathCR = null;
    }

}
