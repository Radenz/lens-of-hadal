using UnityEngine;

public class Quest : MonoBehaviour
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
            EventManager.Instance.CompleteQuest();
        }
    }

    private void StartStep()
    {
        GameObject obj = Instantiate(Data.Steps[_stepIndex]);
        QuestStep questStep = obj.GetComponent<QuestStep>();
        questStep.Quest = this;
    }
}
