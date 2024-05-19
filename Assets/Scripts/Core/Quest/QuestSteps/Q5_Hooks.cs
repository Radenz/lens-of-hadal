using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Q5_Hooks : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _boundCollider;
    [SerializeField]
    private RangeTrigger _enterTrigger;
    [SerializeField]
    private GameObject _anglerfishPrefab;
    [SerializeField]
    private Transform _anglerfishSpawnTransform;

    [SerializeField]
    private GameObject _bossfightBarPrefab;
    private GameObject _bossfightBar;

    private GameObject _anglerfish;

    [SerializeField]
    private GameObject _cutsceneLurePrefab;

    private void Start()
    {
        EventManager.Instance.PlayerDead += OnPlayerDead;
        EventManager.Instance.PlayerRespawned += OnPlayerRespawned;
        _enterTrigger.Entered += OnPlayerEntered;

        SpawnAnglerfish();
    }

    private void OnPlayerDead()
    {
        _boundCollider.enabled = false;
    }

    private void OnPlayerRespawned()
    {
        if (_anglerfish != null) DespawnAnglerfish();
    }

    private void OnPlayerEntered()
    {
        _boundCollider.enabled = true;
        if (_anglerfish == null)
            SpawnAnglerfish();
    }

    public async void SpawnAnglerfish()
    {
        GameObject vCamObject = GameObject.FindGameObjectWithTag("AnglerfishLairVCam");
        GameObject focusVCamObject = GameObject.FindGameObjectWithTag("AnglerfishLairFocusVCam");
        CinemachineVirtualCamera vCam = vCamObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera focusVCam = focusVCamObject.GetComponent<CinemachineVirtualCamera>();

        GameObject cutsceneLure = Instantiate(_cutsceneLurePrefab, _anglerfishSpawnTransform.position, Quaternion.identity);
        focusVCam.LookAt = cutsceneLure.transform;
        CameraManager.Instance.ChooseCamera(focusVCam);

        await Awaitable.WaitForSecondsAsync(3f);

        CameraManager.Instance.ChooseCamera(vCam);

        _anglerfish = Instantiate(_anglerfishPrefab, _anglerfishSpawnTransform.position, Quaternion.identity);
        _bossfightBar = Instantiate(_bossfightBarPrefab);
    }

    public void DespawnAnglerfish()
    {
        Destroy(_anglerfish);
        Destroy(_bossfightBar);
        _anglerfish = null;
    }
}
