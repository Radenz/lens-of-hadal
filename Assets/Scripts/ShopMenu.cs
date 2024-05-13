using UnityEngine;

public class ShopMenu : Singleton<ShopMenu>
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
