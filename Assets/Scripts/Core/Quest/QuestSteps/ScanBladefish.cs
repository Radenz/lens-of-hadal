using UnityEngine;

public class ScanBladefish : QuestStep
{
    [SerializeField]
    private int _target;
    private int _scanned;

    protected override string GetDescription()
    {
        return $"Scan {_target} Bladefishes ({_scanned}/{_target})";
    }

    protected override void Start()
    {
        base.Start();
        _scanned = 0;
        EventManager.Instance.CreatureScanned += OnCreatureScanned;
    }

    private void OnCreatureScanned(string id)
    {
        if (id == Creatures.Bladefish)
        {
            _scanned++;
            if (_scanned > _target)
                _scanned = _target;
        }
        UpdateDescription();
        CheckCompletion();
    }

    protected override void CheckCompletion()
    {
        if (_scanned >= _target) Finish();
    }
}
