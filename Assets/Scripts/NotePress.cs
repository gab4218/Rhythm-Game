public class NotePress : Note, IHittable
{
    public void OnHit()
    {
        EventManager.TriggerEvent(EventType.Hit);
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
