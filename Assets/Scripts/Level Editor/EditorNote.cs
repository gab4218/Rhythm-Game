using UnityEngine;
using UnityEngine.UI;

public class EditorNote : MonoBehaviour
{
    public EditorNoteData data;

    [SerializeField] private Image _selectionImage;


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