using NaughtyAttributes;
using UnityEngine;

// TODO: remove text on finished
public class Scannable : MonoBehaviour
{
    [SerializeField]
    private Creature _creature;

    [SerializeField]
    private GameObject _scanProgressBarPrefab;
    [SerializeField]
    private float _scanDuration = 2;

    [SerializeField]
    private float _scanTime;
    private float ScanProgress => 1 - (_scanTime / _scanDuration);
    private bool _isActivelyScanned;
    private Transform _transform;

    public bool IsScanned { get; private set; }
    public Vector2 Position => _transform.position;

    private GameObject _scanProgressBarContainer;
    private ScanProgressBar _scanProgressBar;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _textOffset = 5;

    private void Awake()
    {
        _transform = transform;
        IsScanned = false;
        _scanTime = _scanDuration;
        _isActivelyScanned = false;
    }

    private void Update()
    {
        if (_isActivelyScanned && !IsScanned)
            UpdateTimeAndColor();

        if (_scanTime < 0 && !IsScanned)
            FinishScan();
    }

    private void UpdateTimeAndColor()
    {
        _scanTime -= Time.deltaTime;
        _scanProgressBarContainer.GetComponent<ScanProgressBar>().SetProgress(ScanProgress);
        EventManager.Instance.UpdateScanProgress(_creature.Id, ScanProgress);
    }

    [Button]
    public void StartScan()
    {
        _isActivelyScanned = true;

        if (_scanProgressBarContainer != null)
        {
            _scanProgressBarContainer.SetActive(true);
            return;
        }
        _scanProgressBarContainer = Instantiate(_scanProgressBarPrefab, transform);
        _scanProgressBar = _scanProgressBarContainer.GetComponent<ScanProgressBar>();
        _scanProgressBar.YOffset = _textOffset;
    }

    public void StopScan()
    {
        _isActivelyScanned = false;
        _scanProgressBarContainer.SetActive(false);
    }

    public void FinishScan()
    {
        IsScanned = true;
        _scanProgressBar.HideText();

        float dna = Random.Range(_creature.DNAPerScan.x, _creature.DNAPerScan.y);
        int energyPowder = Random.Range(_creature.EnergyPowderPerScan.x, _creature.EnergyPowderPerScan.y);
        int seaweed = Random.Range(_creature.SeaweedPerScan.x, _creature.SeaweedPerScan.y);
        int scrapMetal = Random.Range(_creature.ScrapMetalPerScan.x, _creature.ScrapMetalPerScan.y);

        EventManager.Instance.ScanCreature(_creature.Id);
        EventManager.Instance.IncreaseCreatureDNA(_creature.Id, dna);
        EventManager.Instance.RewardPlayer(energyPowder, seaweed, scrapMetal);
        Announcer.Instance.AnnounceScan(_creature.Name, energyPowder, seaweed, scrapMetal);

        LevelManager.Instance.AddExp(_creature.ExpPerScan);
    }
}
