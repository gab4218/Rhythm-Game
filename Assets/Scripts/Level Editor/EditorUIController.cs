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
    
    public EditorNoteSpeeds selectedSpeed = EditorNoteSpeeds.Regular;

    private Image _selectedImage;

    public Color selectColor = new Color(1, 0.9348958f, 0.75f);

    public Color hoverColor;


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
        hoverColor = new Color(selectColor.r, selectColor.g, selectColor.b, 0.4f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenManager.instance.Push("EditorMenu");
        }
    }

    public void Menu() => ScreenManager.instance.Push("EditorMenu");

    public void SelectNote(EditorNoteTypes type) => selectedType = type;

    public void SelectSpeed(EditorNoteSpeeds speed) => selectedSpeed = speed;

    public void SelectButton(Image selectionImage)
    {
        if (_selectedImage == selectionImage)
        {
            if (_selectedImage.color == selectColor)
            {
                _selectedImage.color = new Color(0, 0, 0, 0);
            }
            else
            {
                _selectedImage.color = selectColor;
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
        _selectedImage.color = selectColor;
    }

    public void HoverButton(Image image)
    {
        if (_selectedImage == image) return;
        image.color = hoverColor;
    }

    public void LeaveButton(Image image)
    {
        if (_selectedImage == image) return;
        image.color = new Color(0, 0, 0, 0);
    }

}
