using UnityEngine;

public class Bestiary : Singleton<Bestiary>
{
    public void Show()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
