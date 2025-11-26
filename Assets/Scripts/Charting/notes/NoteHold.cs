public class NoteHold : Note, IHittable
{
    public void OnHit()
    {
        return;
    }

    public void OnHold()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Explode();
    }

    public void OnRelease()
    {
        return;
    }
}
