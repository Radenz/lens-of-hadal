using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    private string _tag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(_tag))
            return;

        CameraManager.Instance.ChooseCamera(_virtualCamera);
    }
}
