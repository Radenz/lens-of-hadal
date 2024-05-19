using UnityEngine;

public class BossfightBar : MonoBehaviour
{
    [SerializeField]
    private Creature _bossCreature;
    [SerializeField]
    private Bar _bar;

    private void Start()
    {
        _bar.Value = 0;
        _bar.MaxValue = 1;
    }

    private void OnEnable()
    {
        EventManager.Instance.ScanProgressUpdated += OnScanProgressUpdated;
    }

    private void OnDisable()
    {
        EventManager.Instance.ScanProgressUpdated -= OnScanProgressUpdated;
    }

    private void OnScanProgressUpdated(string creatureId, float progress)
    {
        if (creatureId == _bossCreature.Id)
        {
            _bar.Value = progress;
        }
    }
}
