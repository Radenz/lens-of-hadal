using UnityEngine;

public class Scannable : MonoBehaviour
{

    [SerializeField]
    private ScanProgressBar _scanProgressBarPrefab;

    private GameObject ScanProgressBarObject => _scanProgressBarPrefab.gameObject;
    private GameObject _scanProgressBar;

    public void StartScan()
    {
        if (_scanProgressBar != null)
        {
            _scanProgressBar.SetActive(true);
            return;
        }
        _scanProgressBar = Instantiate(ScanProgressBarObject, transform);
    }

    public void StopScan()
    {
        _scanProgressBar.SetActive(false);
    }
}