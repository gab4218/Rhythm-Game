using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorSongController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _songs;

    public AudioClip selectedClip;

    public static EditorSongController instance;

    public GameObject songTracker = null;

    [SerializeField] private GameObject _trackerPrefab;

    [SerializeField] private RectTransform _root;

    [SerializeField] private Image _selectionImage;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (songTracker == null) PlaySong(); else StopSong();
        }


        if (songTracker == null) return;

        songTracker.transform.localPosition += Vector3.right * 20f * Time.deltaTime;
        //if (songTracker.transform.localPosition.x > _root.sizeDelta.x) StopSong();
    }

    public void ChangeSong(TMP_Dropdown picker)
    {
        selectedClip = _songs[picker.value];
    }

    public void PlaySong()
    {
        SoundSingleton.instance.SetMusic(selectedClip);
        songTracker = Instantiate(_trackerPrefab, _root);
    }

    public void StopSong()
    {
        SoundSingleton.instance.StopMusic();
        Destroy(songTracker);
        songTracker = null;
    }

    public void Hover()
    {
        _selectionImage.color = EditorUIController.instance.hoverColor;
    }

    public void Leave()
    {
        _selectionImage.color = new(0, 0, 0, 0);
    }

}
