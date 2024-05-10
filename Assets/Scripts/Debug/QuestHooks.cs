using NaughtyAttributes;
using UnityEngine;

public class QuestHooks : MonoBehaviour
{
    public QuestData Quest;

    [Button]
    public void UnlockQuest()
    {
        EventManager.Instance.UnlockQuest(Quest);
    }


    [Button]
    public void CompleteQuest()
    {
        EventManager.Instance.CompleteQuest(Quest);
    }
}
