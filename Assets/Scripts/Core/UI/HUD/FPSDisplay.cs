using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    private void Update()
    {
        float fps = 1f / Time.deltaTime;
        int intFps = Mathf.RoundToInt(fps);
        _label.text = intFps.ToString();
    }
}

