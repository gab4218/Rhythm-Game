using UnityEngine;

public abstract class Note : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected int points;
    private float speed;
    protected bool missed = false;

    public Note Speed(float s)
    {
        speed = s;
        return this;
    }

    public Note StartPos(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        return this;
    }


    protected void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime * 5f;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
        EventManager.TriggerEvent(EventType.Miss);
    }

    //private void Start()
    //{
    //    EventManager.Subscribe(EventType.Death, End);
    //    EventManager.Subscribe(EventType.End, End);
    //}
    //
    //
    //private void End(params object[] paramContainer)
    //{
    //    speed = 0;
    //    EventManager.Unsubscribe(EventType.Death, End);
    //    EventManager.Unsubscribe(EventType.End, End);
    //}
}
