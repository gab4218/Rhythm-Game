using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorNotePlacer : MonoBehaviour
{
    [SerializeField] private Transform _parent, _upperBound, _lowerBound;
    [SerializeField] private EditorNote[] _noteTypeArray;
    private Dictionary<EditorNoteTypes, EditorNote> _noteDictionary = new();

    [SerializeField] private int _horizontalSpacing = 12;
    [SerializeField] private int _verticalSpacing = 64;
    [SerializeField] private int _verticalOffset = 32;
    [SerializeField] private float _boundsScale;


    private PointerEventData _pointerData;

    private List<EditorNote> _level = new();

    private void Start()
    {
        _noteDictionary.Add(EditorNoteTypes.Press, _noteTypeArray[0]);
        _noteDictionary.Add(EditorNoteTypes.Hold, _noteTypeArray[1]);
        _noteDictionary.Add(EditorNoteTypes.Release, _noteTypeArray[2]);
    }

    private void Update()
    {
        if (!EditorUIController.instance.anySelected) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _pointerData = new(EventSystem.current);
            _pointerData.position = Input.mousePosition;

            if (_pointerData.position.y < _lowerBound.position.y || _pointerData.position.y > _upperBound.position.y) return;

            List<RaycastResult> resList = new();
            EventSystem.current.RaycastAll(_pointerData, resList);

            if (resList.Count <= 0) return;
            if (resList[0].gameObject.tag != "Placeable") return;

            _boundsScale = (_upperBound.position.y - _lowerBound.position.y) / 640f;

            var note = Instantiate(_noteDictionary[EditorUIController.instance.selectedType], _pointerData.position / _boundsScale, Quaternion.identity, _parent);

            note.transform.position = new Vector2(_horizontalSpacing * (int)(note.transform.position.x / _horizontalSpacing) + _horizontalSpacing / 2, _verticalSpacing * (int)(note.transform.position.y / _verticalSpacing) + _verticalOffset) * _boundsScale;
            note.type = EditorUIController.instance.selectedType;
            note.speed = EditorUIController.instance.selectedSpeed;
            _level.Add(note);

        }
    }
}
    