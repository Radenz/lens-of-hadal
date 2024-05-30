using UnityEngine;

public class Q1_Movement : QuestStep
{
    [SerializeField]
    private GameObject _movementStick;
    [SerializeField]
    private GameObject _scanStick;
    [SerializeField]
    private GameObject _dashButton;
    [SerializeField]
    private GameObject _mapButton;
    [SerializeField]
    private GameObject _bestiaryButton;

    [SerializeField]
    private GameObject _movementTutorialPrefab;

    private bool _isReached = false;

    protected override void Start()
    {
        _movementStick.SetActive(false);
        _scanStick.SetActive(false);
        _dashButton.SetActive(false);
        _mapButton.SetActive(false);
        _bestiaryButton.SetActive(false);
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
