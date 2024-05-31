using System.Collections.Generic;
using UnityEngine;

public class Q1_MoreMovement : QuestStep
{
    [SerializeField]
    private List<RangeTrigger> _movementTargetTriggers;

    private bool _isReached = false;
    private int _index = 0;

    protected override void Start()
    {
        base.Start();
        TryDisplayNextTarget();
    }

    protected override void CheckCompletion()
    {
        if (_isReached) Finish();
    }

    private void TryDisplayNextTarget()
    {
        if (_index != 0)
        {
            RangeTrigger lastTrigger = _movementTargetTriggers[_index - 1];
            lastTrigger.gameObject.SetActive(false);
        }

        if (_index >= _movementTargetTriggers.Count)
        {
            _isReached = true;
            CheckCompletion();
            return;
        }

        RangeTrigger trigger = _movementTargetTriggers[_index];
        trigger.gameObject.SetActive(true);
        trigger.Entered += TryDisplayNextTarget;

        _index++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CheckCompletion();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CheckCompletion();
    }
}
