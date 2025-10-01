public class NotePress : Note, IHittable
{
    public void OnHit()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Destroy(gameObject);
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
