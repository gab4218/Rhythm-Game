public class NoteHold : Note, IHittable
{
    public void OnHit()
    {
        return;
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
