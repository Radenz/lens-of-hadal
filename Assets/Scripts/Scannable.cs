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
    private int _dnaReward = 40;

    [SerializeField]
    private float _scanTime;
    private bool _isActivelyScanned;
    private Transform _transform;

    public bool IsScanned { get; private set; }
    public Vector2 Position => _transform.position;

    private GameObject ScanProgressBarObject => _scanProgressBarPrefab.gameObject;
    private GameObject _scanProgressBar;

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

        // TODO: check if this part has been discovered
        RewardManager.Instance.RewardDNA(_dnaReward);
    }
}
