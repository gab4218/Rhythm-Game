using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidelineRecorder : MonoBehaviour
{
    public Guidelines gl;
    private float _lastTime = 0;

    void Start()
    {
        gl.ClearGuides();
        GetComponent<AudioSource>().PlayOneShot(gl.song);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            gl.AddGuide(Time.time - _lastTime);
            _lastTime = Time.time;
        }
    }
}
