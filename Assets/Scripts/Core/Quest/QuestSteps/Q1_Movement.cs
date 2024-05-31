using System.Collections.Generic;
using UnityEngine;

public class Q1_Movement : QuestStep
{
    private GameObject _movementStick;
    private GameObject _scanStick;
    private GameObject _dashButton;
    private GameObject _mapButton;
    private GameObject _bestiaryButton;

    [SerializeField]
    private GameObject _introTutorialPrefab;

    [SerializeField]
    private GameObject _movementTutorialPrefab;

    [SerializeField]
    private List<RangeTrigger> _movementTargetTriggers;

    private GameObject _tutorial;

    private bool _isReached = false;
    private int _index = 0;

    protected override void Start()
    {
        _movementStick = GameObject.FindGameObjectWithTag("MovementStick");
        _scanStick = GameObject.FindGameObjectWithTag("ScanStick");
        _dashButton = GameObject.FindGameObjectWithTag("DashButton");
        _mapButton = GameObject.FindGameObjectWithTag("MapButton");
        _bestiaryButton = GameObject.FindGameObjectWithTag("BestiaryButton");

        _movementStick.transform.localScale = Vector3.zero;
        _scanStick.transform.localScale = Vector3.zero;
        _dashButton.transform.localScale = Vector3.zero;
        _mapButton.transform.localScale = Vector3.zero;
        _bestiaryButton.transform.localScale = Vector3.zero;
        base.Start();

        DisplayIntroTutorial();
    }

    protected override void CheckCompletion()
    {
        if (_isReached) Finish();
    }

    private void DisplayIntroTutorial()
    {
        _tutorial = Instantiate(_introTutorialPrefab);
        Typewriter typewriter = _tutorial.GetComponent<Typewriter>();
        typewriter.Completed += DisplayMovementTutorial;
    }

    private void DisplayMovementTutorial()
    {
        Destroy(_tutorial);
        _movementStick.transform.localScale = Vector3.one;
        _mapButton.transform.localScale = Vector3.one;
        _tutorial = Instantiate(_movementTutorialPrefab);
        Typewriter typewriter = _tutorial.GetComponent<Typewriter>();
        typewriter.Completed += BeginMovementQuest;
    }

    private void BeginMovementQuest()
    {
        Destroy(_tutorial);
        TryDisplayNextTarget();
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
