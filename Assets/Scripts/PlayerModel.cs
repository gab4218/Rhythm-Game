using UnityEngine;

public class PlayerModel
{
    private Transform _transform;
    private Transform[] _lanePositions;
    private float _noteDetectionRange;

    private UnityEngine.UI.Image _hpImage;
    public event System.Action onSwitchLane, onMiss, onHitNote;

    private float _laneLerpSpeed;
    private float _maxHealth;
    public float health;
    private float _healthRegen;
    private int _currentLane = 0;
    private Ray _ray;
    private float _originalHeight;

    public PlayerModel(Transform transform, Transform[] lanePositions, UnityEngine.UI.Image img)
    {
        _transform = transform;
        _lanePositions = lanePositions;
        _hpImage = img;
        EventManager.Subscribe(EventType.Miss, MissNote);
        EventManager.Subscribe(EventType.Hit, Hit);
        EventManager.Subscribe(EventType.Death, Unsub);
        EventManager.Subscribe(EventType.End, Unsub);
        _originalHeight = _hpImage.rectTransform.sizeDelta.y;
    }


    public PlayerModel LerpSpeed(float speed = 1f)
    {
        _laneLerpSpeed = speed;
        return this;
    }

    public PlayerModel HP(float hp = 1f, float healtRegen = 0.02f)
    {
        _maxHealth = hp;
        health = _maxHealth;
        _healthRegen = healtRegen;
        return this;
    }

    public PlayerModel Range(float range = 2f)
    {
        _noteDetectionRange = range;
        return this;
    }

    public void OnUpdate()
    {
        float t = 1f - Mathf.Pow(0.2f, Time.deltaTime * _laneLerpSpeed);
        Vector3.Lerp(_transform.position, _lanePositions[_currentLane].position, t);


        _transform.SetPositionAndRotation(Vector3.Lerp(_transform.position, _lanePositions[_currentLane].position, t), Quaternion.Lerp(_transform.rotation, _lanePositions[_currentLane].rotation, t));
        _hpImage.rectTransform.sizeDelta = new Vector2 (_hpImage.rectTransform.sizeDelta.x, health / _maxHealth * _originalHeight);

    }

    public void PressHit()
    {
        _ray = new Ray(_transform.position, _transform.forward);
        if (Physics.Raycast(_ray, out RaycastHit hit, _noteDetectionRange))
        {
            if (hit.collider.TryGetComponent(out IHittable hittable))
            {
                hittable.OnHit();
                
            }
        }
        else
        {
            MissNote();
        }
    }

    public void HoldHit()
    {
        _ray = new Ray(_transform.position, _transform.forward);
        if (Physics.Raycast(_ray, out RaycastHit hit, _noteDetectionRange))
        {
            if (hit.collider.TryGetComponent(out IHittable hittable))
            {
                hittable.OnHold();
                
            }
        }
    }
    
    public void ReleaseHit()
    {
        _ray = new Ray(_transform.position, _transform.forward);
        if (Physics.Raycast(_ray, out RaycastHit hit, _noteDetectionRange))
        {
            if (hit.collider.TryGetComponent(out IHittable hittable))
            {
                hittable.OnRelease();
            }
        }
    }

    public void Hit(params object[] paramContainer)
    {
        onHitNote?.Invoke();
        //Debug.Log("hit");
        if (health < _maxHealth)
        {
            health = Mathf.Min(health + _healthRegen, _maxHealth);
        }
    }

    public void MissNote(params object[] paramContainer)
    {
        health -= 0.05f;
        onMiss?.Invoke();
        if (health <= 0)
        {
            EventManager.TriggerEvent(EventType.Death);
        }
    }

    private void Unsub(params object[] paramContainer)
    {
        EventManager.Unsubscribe(EventType.Miss, MissNote);
        EventManager.Unsubscribe(EventType.Hit, Hit);
        EventManager.Unsubscribe(EventType.Death, Unsub);
        EventManager.Unsubscribe(EventType.End, Unsub);
    }

    public void SwitchLane(float dir)
    {
        if (dir == 0) return;
        onSwitchLane?.Invoke();

        if (dir > 0)
        {
            _currentLane = (_currentLane + 1) % _lanePositions.Length;
        }
        else if (dir < 0)
        {
            _currentLane = _currentLane == 0? (_lanePositions.Length - 1) : (_currentLane - 1);
        }
    }

}

