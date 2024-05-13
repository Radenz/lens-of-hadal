using UnityEngine;

public class QuestMenu : Singleton<QuestMenu>
{
    [SerializeField]
    private Canvas _canvas;

    public void Show()
    {
        _canvas.enabled = true;
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }

    public void Close()
    {
        SceneTransitionSystem.Instance.OpenWorld();
    }
}
