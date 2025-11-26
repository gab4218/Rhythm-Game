public class NotePress : Note, IHittable
{
    public void OnHit()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Explode();
    }

    public void OnHold()
    {
        return;
    }

    public void OnRelease()
    {
        return;
    }
}
