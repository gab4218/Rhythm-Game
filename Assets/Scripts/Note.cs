using UnityEngine;

public abstract class Note : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _particles;
    public float speed;
    protected bool missed = false;

    protected void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime * 5f;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
        EventManager.TriggerEvent(EventType.Miss);
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.Death, Death);
    }


    private void Death()
    {
        speed = 0;
    }
}
