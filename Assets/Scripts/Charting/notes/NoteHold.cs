using UnityEngine;

public class NoteHold : Note, IHittable
{
    [SerializeField] private MeshRenderer _renderer;
    
    public void OnHit()
    {
        return;
    }

    public void OnHold()
    {
        EventManager.TriggerEvent(EventType.Hit, points);
        Explode();
        _renderer.enabled = false;
    }

    public void SetTrail(Transform pos, Transform myPos, Transform prevT)
    {
        if (!prevT) 
        {
            _renderer.enabled = false;
            return;
        }
        Debug.Log("G");
        var z = prevT.position.z - transform.position.z;
        z = Mathf.Abs(z);

        var p = transform.position;
        p.z -= z/2f;

        var s = _renderer.transform.localScale;
        s.z *= z;

        var a = Vector3.SignedAngle(myPos.transform.up, pos.up, transform.forward);

        var d = myPos.localPosition - pos.localPosition;
        
        d.z = 0;
        d = Quaternion.Euler(new(0, 0, -myPos.localEulerAngles.z)) * d;


        if (Mathf.Abs(z) > 15)
        {
            _renderer.enabled = false;
            return;
        }

        _renderer.material.SetFloat("_Angle", a);
        _renderer.material.SetVector("_Pos", -d);

        _renderer.transform.position = p;
        _renderer.transform.localScale = s;

    }

    public void OnRelease()
    {
        return;
    }
}
