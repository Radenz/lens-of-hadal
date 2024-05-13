using NaughtyAttributes;
using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private ScanProgressBar _scanProgressBarPrefab;
    [SerializeField]
    private float _scanDuration = 2;

    [SerializeField]
    private float _scanTime;
    private bool _isActivelyScanned;
    private Transform _transform;

    public bool IsScanned { get; private set; }
    public Vector2 Position => _transform.position;

    private GameObject ScanProgressBarObject => _scanProgressBarPrefab.gameObject;
    private GameObject _scanProgressBar;

    [Header("Rewards")]
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _energyPowder;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _seaweed;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _scrapMetal;
    [SerializeField]
    private int _exp;

    private void Awake()
    {
        _transform = transform;
        IsScanned = false;
        _scanTime = _scanDuration;
        _isActivelyScanned = false;
    }

    private void Update()
    {
        if (_isActivelyScanned)
            _scanTime -= Time.deltaTime;

        if (_scanTime < 0 && !IsScanned)
            FinishScan();
    }

    public void StartScan()
    {
        _isActivelyScanned = true;

        if (_scanProgressBar != null)
        {
            _scanProgressBar.SetActive(true);
            return;
        }
        _scanProgressBar = Instantiate(ScanProgressBarObject, transform);
    }

    public void StopScan()
    {
        _isActivelyScanned = false;
        _scanProgressBar.SetActive(false);
    }

    public void FinishScan()
    {
        IsScanned = true;
        StopScan();

        int energyPowder = Random.Range(_energyPowder.x, _energyPowder.y);
        int seaweed = Random.Range(_seaweed.x, _seaweed.y);
        int scrapMetal = Random.Range(_scrapMetal.x, _scrapMetal.y);

        EventManager.Instance.RewardPlayer(energyPowder, seaweed, scrapMetal);
        Announcer.Instance.AnnounceScan(_name, energyPowder, seaweed, scrapMetal);

        LevelManager.Instance.AddExp(_exp);
    }
}
