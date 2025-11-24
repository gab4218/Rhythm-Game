using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _mainGroup, _shopGroup;
    [SerializeField] float _lerpSpeed = 3f;
    Vector2 _startingPos, _targetPos;
    private void Start()
    {
        _startingPos = transform.position;
        _shopGroup.blocksRaycasts = false;
        _targetPos = _startingPos;
    }

    private void Update()
    {
        float t = 1f - Mathf.Pow(0.3f, Time.deltaTime * _lerpSpeed);
        transform.position = Vector2.Lerp(transform.position, _targetPos, t);
    }

    public void GoToShop()
    {
        _mainGroup.blocksRaycasts = false;
        StartCoroutine(GoToPos(_shopGroup));
        _targetPos = new Vector2(_mainGroup.transform.position.x, _startingPos.y);
    }

    public void ReturnToMain()
    {
        _shopGroup.blocksRaycasts = false;
        StartCoroutine(GoToPos(_mainGroup));
        _targetPos = _startingPos;
    }

    private IEnumerator GoToPos(CanvasGroup cg)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            yield return null;
        }
        cg.blocksRaycasts = true;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
