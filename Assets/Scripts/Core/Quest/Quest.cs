using UnityEngine;

public class Quest
{
    public QuestData Data;
    public bool IsCompleted = false;
    private int _stepIndex;

    public void StartQuest()
    {
        IsCompleted = false;
        _stepIndex = 0;
        StartStep();
    }

    public void Proceed()
    {
        _stepIndex += 1;
        CheckCompletion();
        if (IsCompleted) return;
        EventManager.Instance.HideDisplayQuest();
        StartStep();
    }

    public void CheckCompletion()
    {
        if (Data.Steps.Length == _stepIndex)
        {
            IsCompleted = true;
            EventManager.Instance.CompleteQuest(Data);
            EventManager.Instance.HideDisplayQuest();
            EventManager.Instance.SetQuestDisplay("Quest Completed", "Go back to the ship to claim your reward");
        }
    }

    private void StartStep()
    {
        Object.Instantiate(Data.Steps[_stepIndex]);
    }
}
