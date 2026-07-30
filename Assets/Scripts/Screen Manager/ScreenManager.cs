using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ScreenManager : MonoBehaviour
{

    public static ScreenManager instance;

    private Stack<IScreen> _stack = new();

    [SerializeField] private VisualEffect _buttonVFX;
    [SerializeField] private Image _rawImg;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    public void Pop()
    {
        if (_stack.Count <= 1) return;
        
        _stack.Pop().Free();

        if (_stack.Count > 0)
        {
            _stack.Peek().Activate();
        }

    }

    public void Push(IScreen screen)
    {
        if (_stack.Count > 0)
        {
            _stack.Peek().Deactivate();
        }

        _stack.Push(screen);

        screen.Activate();

    }

    public void Push(string resource)
    {
        Push(Instantiate(Resources.Load<GameObject>(resource)).GetComponent<IScreen>());
    }

    public void Clear()
    {
        while (_stack.Count > 1)
        {
            Pop();
        }
    }

    public void OnClick(Transform pos)
    {
        _rawImg.transform.position = pos.position;
        StartCoroutine(ClickCR());
    }

    private IEnumerator ClickCR()
    {
        _rawImg.enabled = true;

        _buttonVFX.SendEvent("Play");
        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime;
            yield return null;
        }
        _rawImg.enabled = false;

    }
}
