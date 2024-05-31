using NaughtyAttributes;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [HideInInspector]
    public Quest Quest;

    [SerializeField]
    [ResizableTextArea]
    protected string _description;

    public string Description => GetDescription();

    protected virtual void Start()
    {
        Quest = QuestManager.Instance.CurrentQuest;
        UpdateDisplay();
        CheckCompletion();
    }

    protected void UpdateDisplay()
    {
        EventManager.Instance.SetQuestDisplay(Quest.Data.Title, GetDescription());
    }

    protected virtual string GetDescription()
    {
        return _description;
    }

    protected void UpdateDescription()
    {
        EventManager.Instance.SetQuestDisplay(Quest.Data.Title, GetDescription());
    }

    protected void Finish()
    {
        Quest.Proceed();
        Destroy(gameObject);
    }

    protected abstract void CheckCompletion();
}
