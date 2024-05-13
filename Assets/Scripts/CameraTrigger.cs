using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private string _tag;

    [SerializeField]
    private UnityEvent _onTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(_tag))
            return;

        CameraManager.Instance.ChooseCamera(_virtualCamera);
        _onTriggered?.Invoke();
    }
}
