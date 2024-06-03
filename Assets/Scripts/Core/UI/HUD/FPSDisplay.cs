using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    private void Update()
    {
        float fps = Time.deltaTime == 0 ? 60 : 1f / Time.deltaTime;
        int intFps = Mathf.RoundToInt(fps);
        _label.text = intFps.ToString();
    }
}

