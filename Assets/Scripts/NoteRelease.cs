public class NoteRelease : Note, IHittable
{
    public void OnHit()
    {
        return;
    }

    public void OnHold()
    {
        return;
    }

    public void OnRelease()
    {
        EventManager.TriggerEvent(EventType.Hit);
        Destroy(gameObject);
    }
}
