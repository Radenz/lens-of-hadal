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
}
