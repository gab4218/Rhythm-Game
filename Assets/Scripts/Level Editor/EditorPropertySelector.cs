using TMPro;
using UnityEngine;

public class EditorPropertySelector : MonoBehaviour
{
    [SerializeField] private EditorNoteTypes _type;
    
    [SerializeField] private TMP_Dropdown _dropdown;


    public void SelectNote() => EditorUIController.instance.SelectNote(_type);

    public void SelectSpeed() => EditorUIController.instance.SelectSpeed((EditorNoteSpeeds)_dropdown.value);
    
}
