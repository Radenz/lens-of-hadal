using UnityEngine;

public class ScanCreature : QuestStep
{
    [SerializeField]
    private Creature _creature;
    private bool _hasScanned = false;

    protected override void Start()
    {
        base.Start();
        EventManager.Instance.CreatureScanned += OnCreatureScanned;
    }

    private void OnDestroy()
    {
        EventManager.Instance.CreatureScanned -= OnCreatureScanned;
    }

    private void OnCreatureScanned(string id)
    {
        if (_creature.Id == id) _hasScanned = true;
        CheckCompletion();
    }

    protected override void CheckCompletion()
    {
        if (_hasScanned) Finish();
    }
}
