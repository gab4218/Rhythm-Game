using UnityEngine;
using UnityEngine.UI;

public class EditorNote : MonoBehaviour
{
    public EditorNoteData data;

    [SerializeField] private Image _selectionImage;
    [SerializeField] private Color _fastColor, _slowColor;
    [SerializeField] private Image _speedImg;

    private void Start()
    {
        switch (data.speed)
        {
            case EditorNoteSpeeds.Half:
                _speedImg.color = _slowColor;
                break;
            case EditorNoteSpeeds.Regular:
                break;
            case EditorNoteSpeeds.Double:
                _speedImg.color = _fastColor;
                break;
        }
    }

    public void Select(bool real)
    { 
        _selectionImage.color = EditorUIController.instance.selectColor;
        if (!real) return;
        EditorNoteManager.instance.SelectNote(this);
        
    }

    public void SetSpeed(EditorNoteSpeeds newSpeed) =>  data.speed = newSpeed;

    public void Deselect() => _selectionImage.color = new Color(0, 0, 0, 0);

    public void Hover() => _selectionImage.color = EditorUIController.instance.hoverColor;

}


[System.Serializable]
public struct EditorNoteData
{
    public EditorNoteTypes type;
    public EditorNoteSpeeds speed;
    public Vector3 position;
}