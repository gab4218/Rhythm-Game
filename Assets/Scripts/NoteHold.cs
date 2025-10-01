public class NoteHold : Note, IHittable
{
    public void OnHit()
    {
        return;
    }

    public void OnHold()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Destroy(gameObject);
    }

    public void OnRelease()
    {
        return;
    }
}
