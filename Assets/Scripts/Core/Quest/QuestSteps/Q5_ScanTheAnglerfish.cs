using Cinemachine;
using UnityEngine;

public class Q5_ScanTheAnglerfish : QuestStep
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

    private bool _hasScannedAnglerfish = false;
    private bool _isSpawned = false;

    protected override void Start()
    {
        base.Start();

        EventManager.Instance.PlayerDead += OnPlayerDead;
        EventManager.Instance.PlayerRespawned += OnPlayerRespawned;
        _enterTrigger.Entered += OnPlayerEntered;

        EventManager.Instance.CreatureScanned += OnCreatureScanned;

        SpawnAnglerfish();
    }

    private void OnDestroy()
    {
        EventManager.Instance.PlayerDead -= OnPlayerDead;
        EventManager.Instance.PlayerRespawned -= OnPlayerRespawned;
        EventManager.Instance.CreatureScanned -= OnCreatureScanned;
    }

    protected override string GetDescription()
    {
        return "Scan The Mutant Anglerfish";
    }

    private void OnCreatureScanned(string id)
    {
        if (id == Creatures.MutantAnglerfish)
            _hasScannedAnglerfish = true;
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
        if (!_isSpawned)
            SpawnAnglerfish();
    }

    public async void SpawnAnglerfish()
    {
        _isSpawned = true;

        GameObject vCamObject = GameObject.FindGameObjectWithTag("AnglerfishLairVCam");
        GameObject focusVCamObject = GameObject.FindGameObjectWithTag("AnglerfishLairFocusVCam");
        CinemachineVirtualCamera vCam = vCamObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera focusVCam = focusVCamObject.GetComponent<CinemachineVirtualCamera>();

        GameObject cutsceneLure = Instantiate(_cutsceneLurePrefab, _anglerfishSpawnTransform.position, Quaternion.identity);
        focusVCam.LookAt = cutsceneLure.transform;
        CameraManager.Instance.ChooseCamera(focusVCam);

        await Awaitable.WaitForSecondsAsync(2f);

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

    protected override void CheckCompletion()
    {
        if (_hasScannedAnglerfish)
            Finish();
    }
}
