using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [HideInInspector]
    public Quest Quest;

    [SerializeField]
    protected string _description;

    protected virtual void Start()
    {
        EventManager.Instance.SetQuestDisplay(Quest.Data.Title, GetDescription());
        CheckCompletion();
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

    protected abstract bool CheckCompletion();
}
