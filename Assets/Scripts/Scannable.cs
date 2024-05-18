using NaughtyAttributes;
using UnityEngine;

// TODO: store Creature SO instead
// TODO: remove text on finished
public class Scannable : MonoBehaviour
{
    [Header("Rewards")]
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2 _dna;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _energyPowder;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _seaweed;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _scrapMetal;
    [SerializeField]
    private int _exp;

    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private ScanProgressBar _scanProgressBarPrefab;
    [SerializeField]
    private float _scanDuration = 2;

    [SerializeField]
    private float _scanTime;
    private float ScanProgress => 1 - (_scanTime / _scanDuration);
    private bool _isActivelyScanned;
    private Transform _transform;

    public bool IsScanned { get; private set; }
    public Vector2 Position => _transform.position;

    private GameObject ScanProgressBarObject => _scanProgressBarPrefab.gameObject;
    private GameObject _scanProgressBar;

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
        _scanProgressBar.GetComponent<ScanProgressBar>().SetProgress(ScanProgress);
    }

    [Button]
    public void StartScan()
    {
        _isActivelyScanned = true;

        if (_scanProgressBar != null)
        {
            _scanProgressBar.SetActive(true);
            return;
        }
        _scanProgressBar = Instantiate(ScanProgressBarObject, transform);
        ScanProgressBar bar = _scanProgressBar.GetComponent<ScanProgressBar>();
        bar.YOffset = _textOffset;
    }

    public void StopScan()
    {
        _isActivelyScanned = false;
        _scanProgressBar.SetActive(false);
    }

    public void FinishScan()
    {
        IsScanned = true;

        float dna = Random.Range(_dna.x, _dna.y);
        int energyPowder = Random.Range(_energyPowder.x, _energyPowder.y);
        int seaweed = Random.Range(_seaweed.x, _seaweed.y);
        int scrapMetal = Random.Range(_scrapMetal.x, _scrapMetal.y);

        EventManager.Instance.ScanCreature(_name);
        EventManager.Instance.IncreaseCreatureDNA(_name, dna);
        EventManager.Instance.RewardPlayer(energyPowder, seaweed, scrapMetal);
        Announcer.Instance.AnnounceScan(_name, energyPowder, seaweed, scrapMetal);

        LevelManager.Instance.AddExp(_exp);
    }
}
