using UnityEngine;

public class ScanCreature : QuestStep
{
    [SerializeField]
    private string _name;
    private bool _hasScanned = false;

    protected override void Start()
    {
        base.Start();
        EventManager.Instance.CreatureScanned += OnCreatureScanned;
    }

    private void OnCreatureScanned(string name)
    {
        if (name == _name) _hasScanned = true;
        CheckCompletion();
    }

    protected override void CheckCompletion()
    {
        if (_hasScanned) Finish();
    }
}
