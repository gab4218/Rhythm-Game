using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class LevelMover : MonoBehaviour
{
    private bool _holding = false;
    private float _startingYPos;
    private float _startingYCanvas;
    private bool _devMode = false;

    [SerializeField] private RectTransform _myTransform;
    [SerializeField] private RectTransform _containerTransform;
    [SerializeField] private float _lerpSpeed = 3f;
    [SerializeField] private Transform[] _children;
    private List<Transform> _startingActiveChildren = new();

    private void Start()
    {
        if (transform.childCount > 5)
        {
            int i = 0;
            foreach(Transform t in _children)
            {
                if (t.gameObject.activeSelf)
                {
                    _startingActiveChildren.Add(t);
                    i++;
                    if(i > 5) _myTransform.sizeDelta += Vector2.up * 125;
                }
            }
        }
        RemoteConfigService.Instance.FetchCompleted += DevMode;
    }

    void Update()
    {
        if (_holding)
        {

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            float currentY = Input.mousePosition.y - _startingYPos;
#elif UNITY_ANDROID
            float currentY = _startingYPos - Input.touches[0].position.y;
#endif

            Vector2 newPos = (_startingYCanvas + currentY) * Vector2.up;
            LerpPos(newPos);
        }
        else
        {
            BackToBounds();
        }
    }

    public void Hold(bool hold)
    {
        if (hold)
        {
            _startingYCanvas = _myTransform.anchoredPosition.y;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            _startingYPos = Input.mousePosition.y;
#elif UNITY_ANDROID
            _startingYPos = Input.touches[0].position.y;
#endif
            //Debug.Log((_startingYPos).ToString());
        }
        _holding = hold;
    }

    private void BackToBounds()
    {
        if ((_myTransform.anchoredPosition.y) < 0)
        {
            Vector2 newPos = new Vector2(_myTransform.anchoredPosition.x, 0);
            LerpPos(newPos);
        } 
        else if ((_myTransform.anchoredPosition.y) > (_myTransform.sizeDelta.y - _containerTransform.sizeDelta.y))
        {
            Vector2 newPos = new Vector2(_myTransform.anchoredPosition.x, _myTransform.sizeDelta.y - _containerTransform.sizeDelta.y);
            LerpPos(newPos);
        }
    }

    private void DevMode(ConfigResponse configResponse)
    {
        bool _temp = _devMode;
        _devMode = RemoteConfigService.Instance.appConfig.GetBool("DevMode");

        if (_temp == _devMode) return;

        int i = 0;

        if (_devMode)
        {
            foreach (Transform t in _children)
            {
                t.gameObject.SetActive(true);
                i++;
                if (i > 5) _myTransform.sizeDelta += Vector2.up * 125;        
            }
        }
        else
        {
            foreach (Transform t in _children)
            {
                t.gameObject.SetActive(false);
                i++;
                if (i > 5) _myTransform.sizeDelta -= Vector2.up * 125;
            }
            foreach (Transform t in _startingActiveChildren)
            {
                t.gameObject.SetActive(true);
            }
        }
    }

    private void LerpPos(Vector2 pos)
    {
        float t = 1 - Mathf.Pow(0.2f, Time.deltaTime * _lerpSpeed);
        _myTransform.anchoredPosition = Vector2.Lerp(_myTransform.anchoredPosition, pos, t);
    }
}
