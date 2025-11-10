using Unity.Services.RemoteConfig;
using UnityEngine;

public class EditorDetect : MonoBehaviour
{
    [SerializeField] private GameObject _editorButton;
    private bool _enabled = false;
    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += ChangeVisibility;
        _editorButton.SetActive(_enabled);
    }
    private void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= ChangeVisibility;
    }

    private void ChangeVisibility(ConfigResponse configResponse)
    {
        _enabled = RemoteConfigService.Instance.appConfig.GetBool("ShowEditorButton");

        _editorButton.SetActive(_enabled);
    }
}
