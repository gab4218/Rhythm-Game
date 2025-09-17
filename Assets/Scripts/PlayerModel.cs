using UnityEngine;

public class PlayerModel
{
    private Transform _transform;
    private Transform[] _lanePositions;
    private float _noteDetectionRange;

    private UnityEngine.UI.Image _hpImage;
    public event System.Action onSwitchLane, onMiss, onHitNote;

    private float _iFrames;
    private float _iTime;
    private float _laneLerpSpeed = 1;
    private float _maxHealth = 1f;
    public float health = 1;
    private float _healthRegen = 0.02f;
    private int _currentLane = 0;
    private int _lastLane = 0;
    private Ray _ray;
    private float _originalHeight;

    public PlayerModel(Transform transform, Transform[] lanePositions, UnityEngine.UI.Image img, float iFrames = 0.5f, float laneInterpolationSpeed = 10,float maxHP = 1f, float noteDetectionRange = 2f)
    {
        _transform = transform;
        _lanePositions = lanePositions;
        _hpImage = img;
        _iFrames = iFrames;
        _laneLerpSpeed = laneInterpolationSpeed;
        _maxHealth = maxHP;
        health = maxHP;
        _noteDetectionRange = noteDetectionRange;
        EventManager.Subscribe(EventType.Miss, MissNote);
        EventManager.Subscribe(EventType.Hit, Hit);
        _originalHeight = _hpImage.rectTransform.sizeDelta.y;
        
    }


    public void OnUpdate()
    {
        float t = 1f - Mathf.Pow(0.2f, Time.deltaTime * _laneLerpSpeed);
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

    public void Hit()
    {
        onHitNote?.Invoke();
        Debug.Log("hit");
        if (health < _maxHealth)
        {
            health = Mathf.Min(health + _healthRegen, _maxHealth);
        }
    }

    public void MissNote()
    {
        health -= 0.05f;
        onMiss?.Invoke();
        if (health <= 0)
        {
            EventManager.TriggerEvent(EventType.Death);
        }
    }

    public void SwitchLane(float dir)
    {
        if (dir == 0) return;
        onSwitchLane?.Invoke();

        if (dir > 0)
        {
            _lastLane = _currentLane;
            _currentLane = (_currentLane + 1) % _lanePositions.Length;
        }
        else if (dir < 0)
        {
            _lastLane = _currentLane;
            _currentLane = _currentLane == 0? (_lanePositions.Length - 1) : (_currentLane - 1);
        }
    }

}

