using System.Collections.Generic;
using UnityEngine;

public class Q1_Scan : QuestStep
{
    private GameObject _scanStick;
    private GameObject _bestiaryButton;

    [SerializeField]
    private GameObject _scanTutorialPrefab;

    [SerializeField]
    private GameObject _bestiaryTutorialPrefab;

    private GameObject _tutorial;

    private bool _hasScannedPiranha = false;

    protected override void Start()
    {
        _scanStick = GameObject.FindGameObjectWithTag("ScanStick");
        _bestiaryButton = GameObject.FindGameObjectWithTag("BestiaryButton");

        base.Start();

        EventManager.Instance.CreatureScanned += OnCreatureScanned;
        DisplayScanTutorial();
    }

    protected override void CheckCompletion()
    {
        if (_hasScannedPiranha) Finish();
    }

    private void DisplayScanTutorial()
    {
        _scanStick.transform.localScale = Vector3.one;
        _tutorial = Instantiate(_scanTutorialPrefab);
        Typewriter typewriter = _tutorial.GetComponent<Typewriter>();
        typewriter.Completed += DestroyTutorial;
    }

    private void DisplayBestiaryTutorial()
    {
        _bestiaryButton.transform.localScale = Vector3.one;
        _tutorial = Instantiate(_bestiaryTutorialPrefab);
        Typewriter typewriter = _tutorial.GetComponent<Typewriter>();
        typewriter.Completed += DestroyTutorial;
    }

    private void DestroyTutorial()
    {
        Destroy(_tutorial);
        CheckCompletion();
    }

    private void OnCreatureScanned(string id)
    {
        if (id == Creatures.Piranha)
        {
            EventManager.Instance.CreatureScanned -= OnCreatureScanned;
            _hasScannedPiranha = true;
            DisplayBestiaryTutorial();
        }
    }
}
