using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [HideInInspector]
    public Quest Quest;

    [SerializeField]
    protected string _description;

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

    protected void Finish()
    {
        Quest.Proceed();
        Destroy(gameObject);
    }

    protected abstract void CheckCompletion();
}
