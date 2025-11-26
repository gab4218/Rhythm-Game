public class NoteRelease : Note, IHittable
{
    public void OnHit()
    {
        EventManager.TriggerEvent(EventType.Miss);
    }

    public void OnHold()
    {
        return;
    }

    public void OnRelease()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Explode();
    }
}
