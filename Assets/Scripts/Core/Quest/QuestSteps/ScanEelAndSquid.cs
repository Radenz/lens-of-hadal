using UnityEngine;

public class ScanEelAndSquid : QuestStep
{
    [SerializeField]
    private int _targetEel;
    [SerializeField]
    private int _targetSquid;

    private int _scannedEel;
    private int _scannedSquid;

    protected override string GetDescription()
    {
        return $"Scan {_targetEel} Electric Eels ({_scannedEel}/{_targetEel})\nScan {_targetSquid} Giant Squids ({_scannedSquid}/{_targetSquid})";
    }

    protected override void Start()
    {
        base.Start();
        _scannedEel = 0;
        _scannedSquid = 0;
        EventManager.Instance.CreatureScanned += OnCreatureScanned;
    }

    private void OnCreatureScanned(string id)
    {
        if (id == Creatures.ElectricEel)
        {
            _scannedEel++;
            if (_scannedEel > _targetEel)
                _scannedEel = _targetEel;
        }

        if (id == Creatures.GiantSquid)
        {
            _scannedSquid++;
            if (_scannedSquid > _targetSquid)
                _scannedSquid = _targetSquid;
        }

        UpdateDescription();
        CheckCompletion();
    }

    protected override void CheckCompletion()
    {
        if (_scannedSquid >= _targetSquid && _scannedEel >= _targetEel) Finish();
    }
}
