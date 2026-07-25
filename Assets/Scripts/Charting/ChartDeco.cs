using UnityEngine;

public class ChartDeco : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position -= Vector3.forward * speed * 5f * Time.deltaTime;
        if (transform.position.z < -10) Destroy(gameObject);
    }
}
