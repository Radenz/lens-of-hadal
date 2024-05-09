using UnityEngine;

public class QuestMenu : Singleton<QuestMenu>
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
