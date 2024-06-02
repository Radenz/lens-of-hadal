using NaughtyAttributes;
using UnityEngine;

public class StaticScannable : MonoBehaviour
{
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private int _expReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _energyPowderReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _seaweedReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _scrapMetalReward;

    [SerializeField]
    private string _name;

    [SerializeField]
    private GameObject _scanProgressBarPrefab;
    [SerializeField]
    private float _scanDuration = 2;
    [SerializeField]
    private float _regenDuration = 60;

    private float _scanTime;
    private float ScanProgress => 1 - (_scanTime / _scanDuration);
    private bool _isActivelyScanned;
    private Transform _transform;

    public bool IsScanned { get; private set; }
    public Vector2 Position => _transform.position;

    private GameObject _scanProgressBarContainer;
    private ScanProgressBar _scanProgressBar;

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

        int energyPowder = _energyPowderReward.PickBetween();
        int seaweed = _seaweedReward.PickBetween();
        int scrapMetal = _scrapMetalReward.PickBetween();

        Announcer.Instance.AnnounceObjectScan(_sprite, energyPowder, seaweed, scrapMetal);
        LevelManager.Instance.AddExp(_expReward);

        Timer.Instance.SetTimer(Regenerate, _regenDuration);
    }

    private void Regenerate()
    {
        Destroy(_scanProgressBarContainer);
        _scanProgressBarContainer = null;
    }
}
