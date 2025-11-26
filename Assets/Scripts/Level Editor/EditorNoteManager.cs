using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorNoteManager : MonoBehaviour
{
    [SerializeField] private RectTransform _parent, _upperBound, _lowerBound;
    [SerializeField] private EditorNote[] _noteTypeArray;
    [SerializeField] private Transform _rightmost;
    private Dictionary<EditorNoteTypes, EditorNote> _noteDictionary = new();
    
    [SerializeField] private int _horizontalSpacing = 12;
    [SerializeField] private int _verticalSpacing = 64;
    [SerializeField] private int _verticalOffset = 32;
    [SerializeField] private float _boundsScale;

    private PointerEventData _pointerData;

    private List<EditorNote> _level = new();

    private Memento _undoHistory = new();
    private Memento _redoHistory = new();

    private EditorNote _selectedNote = default;

    public static EditorNoteManager instance;


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
        
        _noteDictionary.Add(EditorNoteTypes.Press, _noteTypeArray[0]);
        _noteDictionary.Add(EditorNoteTypes.Hold, _noteTypeArray[1]);
        _noteDictionary.Add(EditorNoteTypes.Release, _noteTypeArray[2]);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete)) DestroyNote();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Recall(_undoHistory, ref _redoHistory);
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Recall(_redoHistory, ref _undoHistory);
        }

        if (!EditorUIController.instance.anySelected) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetNote(Input.mousePosition, "Placeable", true);
        }

        

    }

    private void PlaceNote()
    {
        RecordState();

        var note = Instantiate(_noteDictionary[EditorUIController.instance.selectedType], _pointerData.position / _boundsScale, Quaternion.identity, _parent);

        note.transform.position = new Vector2(_horizontalSpacing * (int)(note.transform.position.x / _horizontalSpacing) + _horizontalSpacing / 2, _verticalSpacing * (int)(note.transform.position.y / _verticalSpacing) + _verticalOffset) * _boundsScale;
        Vector2 local = _parent.InverseTransformPoint(note.transform.position);

        note.transform.localPosition = new Vector2(local.x - (local.x % _horizontalSpacing) + _horizontalSpacing/2, local.y);


        note.data.type = EditorUIController.instance.selectedType;
        note.data.speed = EditorUIController.instance.selectedSpeed;
        note.data.position = note.transform.position;

        _level.Add(note);

        if (note.transform.position.x > _rightmost.position.x)
        {
            _rightmost = note.transform;
            _parent.sizeDelta = new Vector2(_rightmost.localPosition.x + 1910, _parent.sizeDelta.y);
            
        }

        SelectNote(note);
        
    }

    public void SelectNote(EditorNote note)
    {
        if (_selectedNote != null)
        {
            if (_selectedNote != note)
            {
                _selectedNote.Deselect();
            }
        }

        _selectedNote = note;
        if (_selectedNote != null)
        {
            Debug.Log("selected");
            _selectedNote.Select(false);
        }
    }

    public void DestroyNote()
    {
        Debug.Log(_selectedNote == null);
        if (_selectedNote == null) return;
        Debug.Log("be");
        RecordState();
        _level.Remove(_selectedNote);
        if (_selectedNote.transform == _rightmost)
        {
            if (_level.Count > 0)
            {
                Transform lm = _level[0].transform;
                foreach (EditorNote note in _level)
                {
                    if (note.transform.position.x > lm.position.x)
                    {
                        lm = note.transform;
                    }
                }
                _rightmost = lm;
                _parent.sizeDelta = new Vector2(_rightmost.localPosition.x + 1900, _parent.sizeDelta.y);
            }

        }
        Destroy(_selectedNote.gameObject);
        _selectedNote = null;
    }

    private void RecordState()
    {
        List<EditorNoteData> levelCopy = new();

        foreach (var note in _level)
        {
            levelCopy.Add(note.data);
        }

        _undoHistory.Record(levelCopy, _selectedNote != null? _selectedNote.data : null);

        if(_redoHistory.CanRemember()) _redoHistory.Clear();
    }

    private void SetNote(Vector3 pos, string tag, bool place)
    {
        _pointerData = new(EventSystem.current);
        _pointerData.position = pos;

        if (_pointerData.position.y < _lowerBound.position.y || _pointerData.position.y > _upperBound.position.y) return;

        List<RaycastResult> resList = new();
        EventSystem.current.RaycastAll(_pointerData, resList);

        if (resList.Count <= 0) return;
        if (resList[0].gameObject.tag == tag)
        {
            _boundsScale = (_upperBound.position.y - _lowerBound.position.y) / 640f;
            if (place) PlaceNote();
            else
            {
                var n = resList[0].gameObject.GetComponent<EditorNote>();
                SelectNote(n);
            }
        }
    }


    /// <summary>
    /// This function allows the user to return to the previous state of a Memento and record the current state to another.
    /// The parameter 'reader' is the Memento which is being returned to and 'writer' is the one which is being recorded to.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="writer"></param>
    public void Recall(Memento reader, ref Memento writer)
    {
        if (!reader.CanRemember()) return;

        var snapshot = reader.Remember();
        var level = (List<EditorNoteData>)snapshot[0];
        EditorNoteData selected = new();


        if (snapshot[1] != null)
        {
            selected = (EditorNoteData)snapshot[1];
        }

        List<EditorNoteData> levelCopy = new();

        foreach (var note in _level)
        {
            levelCopy.Add(note.data);
        }

        writer.Record(levelCopy, _selectedNote != null ? _selectedNote.data : null);

        if(level.Count > _level.Count)
        {
            var note = Instantiate(_noteDictionary[selected.type], selected.position, Quaternion.identity, _parent);

            note.data.type = EditorUIController.instance.selectedType;
            note.data.speed = EditorUIController.instance.selectedSpeed;
            note.data.position = note.transform.position;

            _level.Add(note);
            SelectNote(note);
            return;
        }
        else if(level.Count < _level.Count)
        {
            Destroy(_level[_level.Count - 1].gameObject);
            
            _level.RemoveAt(_level.Count - 1);

            SetNote(selected.position, "Note", false);

            return;
        }

    }
    


}
    