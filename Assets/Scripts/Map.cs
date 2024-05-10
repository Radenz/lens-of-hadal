using UnityEngine;

public class Map : Singleton<Map>
{
    [SerializeField]
    private Canvas _canvas;

    public void Show()
    {
        // _canvas.enabled = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        // _canvas.enabled = false;
    }
}
