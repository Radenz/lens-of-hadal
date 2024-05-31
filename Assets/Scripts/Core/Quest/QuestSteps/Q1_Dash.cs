using System.Collections.Generic;
using UnityEngine;

public class Q1_Dash : QuestStep
{
    private GameObject _dashButton;

    [SerializeField]
    private GameObject _dashTutorialPrefab;
    [SerializeField]
    private GameObject _outroTutorialPrefab;

    private GameObject _tutorial;

    private bool _hasScannedEel = false;

    protected override void Start()
    {
        _dashButton = GameObject.FindGameObjectWithTag("DashButton");

        base.Start();

        EventManager.Instance.CreatureScanned += OnCreatureScanned;
        DisplayDashTutorial();
    }

    protected override void CheckCompletion()
    {
        if (_hasScannedEel)
        {
            EventManager.Instance.EnableCreatures();
            Finish();
        }
    }

    private void DisplayDashTutorial()
    {
        _dashButton.transform.localScale = Vector3.one;
        _tutorial = Instantiate(_dashTutorialPrefab);
        Typewriter typewriter = _tutorial.GetComponent<Typewriter>();
        typewriter.Completed += DestroyTutorial;
    }

    private void DisplayOutroTutorial()
    {
        _dashButton.transform.localScale = Vector3.one;
        _tutorial = Instantiate(_outroTutorialPrefab);
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
        if (id == Creatures.ElectricEel)
        {
            EventManager.Instance.CreatureScanned -= OnCreatureScanned;
            _hasScannedEel = true;
            EventManager.Instance.DisableCreatures();
            DisplayOutroTutorial();
        }
    }
}
