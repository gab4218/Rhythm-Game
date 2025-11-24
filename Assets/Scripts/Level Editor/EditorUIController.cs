using UnityEngine;
using UnityEngine.UI;

public enum EditorNoteTypes
{
    Press,
    Hold,
    Release
}

public enum EditorNoteSpeeds
{
    Half,
    Regular,
    Double
}

public class EditorUIController : MonoBehaviour
{
    public static EditorUIController instance;


    public bool anySelected = false;

    public EditorNoteTypes selectedType;
    
    public EditorNoteSpeeds selectedSpeed;

    private Image _selectedImage;

    private Color _selectColor = new Color(1, 0.9348958f, 0.75f);


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

    public void SelectNote(EditorNoteTypes type)
    {
        selectedType = type;
    }

    public void SelectSpeed(EditorNoteSpeeds speed)
    {
        selectedSpeed = speed;
    }

    public void SelectButton(Image selectionImage)
    {
        if (_selectedImage == selectionImage)
        {
            if (_selectedImage.color == _selectColor)
            {
                _selectedImage.color = new Color(0, 0, 0, 0);
            }
            else
            {
                _selectedImage.color = _selectColor;
            }
            anySelected = !anySelected;
            return;
        }

        if (_selectedImage != null)
        {
            _selectedImage.color = new Color(0, 0, 0, 0);
        }
        anySelected = true;
        _selectedImage = selectionImage;
        _selectedImage.color = _selectColor;
    }

    public void HoverButton(Image image)
    {
        if (_selectedImage == image) return;
        image.color = new Color(_selectColor.r, _selectColor.g, _selectColor.b, 0.4f);
    }

    public void LeaveButton(Image image)
    {
        if (_selectedImage == image) return;
        image.color = new Color(0, 0, 0, 0);
    }

}
