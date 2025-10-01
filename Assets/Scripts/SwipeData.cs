using UnityEngine;

public struct SwipeData
{
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 direction
    {
        get
        {
            return (endPos - startPos).normalized;
        }
    }

    public bool swiping;

    public void StartSwipe(Vector2 pos)
    {
        startPos = pos;
        endPos = default;
        swiping = true;
    }

    public void EndSwipe(Vector2 pos)
    {
        endPos = pos;
        swiping = false;
    }
}
