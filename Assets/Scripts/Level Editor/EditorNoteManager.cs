using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorNoteManager : MonoBehaviour
{
    [SerializeField] private RectTransform _parent, _upperBound, _lowerBound;
    [SerializeField] private EditorNote[] _noteTypeArray;
    [SerializeField] private Note[] _noteArray;
    [SerializeField] private Transform _rightmost;
    private Dictionary<EditorNoteTypes, EditorNote> _noteDictionary = new();
    private Dictionary<EditorNoteTypes, Note> _editorToChartNotes = new();

    [SerializeField] private int _horizontalSpacing = 12;
    [SerializeField] private int _verticalSpacing = 64;
    [SerializeField] private float _boundsScale;

    [SerializeField] private ChartData _tempChart;

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

        _editorToChartNotes.Add(EditorNoteTypes.Press, _noteArray[0]);
        _editorToChartNotes.Add(EditorNoteTypes.Hold, _noteArray[1]);
        _editorToChartNotes.Add(EditorNoteTypes.Release, _noteArray[2]);
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


        if (Input.GetKeyDown(KeyCode.L))
        {
            Convert();
        }

        if (!EditorUIController.instance.anySelected) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetNote(Input.mousePosition, "Placeable", true);
        }

        
    }

    private void OnDestroy()
    {
        _tempChart = new();
        Convert();
    }

    public void Convert()
    {
        if (_level.Count <= 0) return;

        _tempChart.notes = new NoteData[_level.Count];

        _level = _level.SortByTime();

        Dictionary<EditorNote, float> timeDic = _level.CalculateTimes(0.005f);

        _level = _level.SortByTime(timeDic);

        Dictionary<EditorNote, float> speeds = _level.CalculateSpeeds();

        Dictionary<EditorNote, int> lanes = _level.CalculateLanes();

        _tempChart.notes[0] = new NoteData();
        _tempChart.notes[0].note = _editorToChartNotes[_level[0].data.type];
        _tempChart.notes[0].delayFromLast = timeDic[_level[0]];
        _tempChart.notes[0].lane = lanes[_level[0]];
        _tempChart.notes[0].noteSpeed = speeds[_level[0]];

        for (int i = 1; i < _level.Count; i++)
        {
            _tempChart.notes[i] = new NoteData();
            _tempChart.notes[i].note = _editorToChartNotes[_level[i].data.type];
            _tempChart.notes[i].delayFromLast = timeDic[_level[i]] - timeDic[_level[i-1]];
            Debug.Log("current" + timeDic[_level[i]] + " last" + timeDic[_level[i-1]]);
            _tempChart.notes[i].lane = lanes[_level[i]];
            _tempChart.notes[i].noteSpeed = speeds[_level[i]];
        }
        _tempChart.song = EditorSongController.instance.selectedSong;
        _tempChart.name = _tempChart.song.name;
        SaveManager.SaveLevel(_tempChart);
        //Destroy(_tempChart);
    }

    private void PlaceNote()
    {
        RecordState();

        var note = Instantiate(_noteDictionary[EditorUIController.instance.selectedType], _pointerData.position, Quaternion.identity, _parent);

        //note.transform.position = new Vector2(_horizontalSpacing * (int)(note.transform.position.x / _horizontalSpacing) + _horizontalSpacing / 2, _verticalSpacing * (int)(note.transform.position.y / _verticalSpacing)) * _boundsScale;
        Vector2 local = _parent.InverseTransformPoint(note.transform.position);

        note.transform.localPosition = new Vector2(local.x - (local.x % _horizontalSpacing) + _horizontalSpacing/2, local.y - (local.y % _verticalSpacing) + _verticalSpacing / 2);

        Debug.Log((local.y % _verticalSpacing));
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
        if (resList[0].gameObject.CompareTag(tag))
        {
            _boundsScale = (_upperBound.position.y - _lowerBound.position.y) / 640f;
            if (place) PlaceNote();
            else
            {
                var n = resList[0].gameObject.GetComponent<EditorNote>();
                _rightmost = n.transform;
                SelectNote(n);
            }
        }
    }


    public void Undo() => Recall(_undoHistory, ref _redoHistory);
    public void Redo() => Recall(_redoHistory, ref _undoHistory);

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
    