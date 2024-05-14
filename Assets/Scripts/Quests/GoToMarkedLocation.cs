using UnityEngine;

public class GoToMarkedLocation : QuestStep
{
    [SerializeField]
    private CircleCollider2D _range;
    private bool _isReached = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckCompletion()
    {
        if (_isReached) Finish();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _isReached = true;
        CheckCompletion();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CheckCompletion();
    }
}
