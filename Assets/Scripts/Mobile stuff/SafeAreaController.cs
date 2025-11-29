using UnityEngine;

public class SafeAreaController : MonoBehaviour
{
    /*
#if UNITY_ANDROID
    [SerializeField] private Canvas _canvas;
    private RectTransform _rectTransform;
    private Rect _lastSafeArea = Rect.zero;
    private Vector2 _minAnchor;
    private Vector2 _maxAnchor;

    private void Awake()
    {
        if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void AdjustSafeArea()
    {
        _lastSafeArea = Screen.safeArea;

        _minAnchor = _lastSafeArea.position;
        _maxAnchor = _minAnchor + _lastSafeArea.size;

        _minAnchor.x /= Screen.width;
        _maxAnchor.x /= Screen.width;
        _minAnchor.y /= Screen.height;
        _maxAnchor.y /= Screen.height;

        _rectTransform.anchorMin = _minAnchor;
        _rectTransform.anchorMax = _maxAnchor;
        Debug.Log("aa");
    }

    private void LateUpdate()
    {
        if (_lastSafeArea != Screen.safeArea) AdjustSafeArea();
    }
#endif
    */
}
