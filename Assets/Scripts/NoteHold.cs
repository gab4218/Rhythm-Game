public class NoteHold : Note, IHittable
{
    public void OnHit()
    {
        EventManager.TriggerEvent(EventType.Hit);
        Destroy(gameObject);
    }

    public void OnHold()
    {
        EventManager.TriggerEvent(EventType.Hit);
        Destroy(gameObject);
    }

    public void OnRelease()
    {
        return;
    }
}
