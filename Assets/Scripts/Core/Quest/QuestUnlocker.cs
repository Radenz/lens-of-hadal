using UnityEngine;

public class QuestUnlocker : MonoBehaviour
{
    [SerializeField]
    private QuestData _quest;

    private void Start()
    {
        EventManager.Instance.UnlockQuest(_quest);
    }
}
