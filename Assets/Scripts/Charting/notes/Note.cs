using UnityEngine;
using UnityEngine.VFX;

public abstract class Note : MonoBehaviour
{
    [SerializeField] protected int points;
    [SerializeField] protected VisualEffect explosion;
    [SerializeField] protected VisualEffect jet;
    [SerializeField] protected Renderer[] myRenderers;

    private float speed;
    protected bool missed = false;

    public Note Speed(float s)
    {
        speed = s;
        return this;
    }

    public Note StartPos(Vector3 pos)
    {
        transform.position = pos;
        //transform.rotation = rot;
        return this;
    }


    protected void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime * 5f;
        transform.up = Vector3.up;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent(EventType.Miss);
    }

    protected virtual void Explode()
    {
        explosion.SendEvent("Death");
        jet.SendEvent("Death");
        foreach (var r in myRenderers) r.enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("Death", 2f);
    }

    protected void Death() => Destroy(gameObject);

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
