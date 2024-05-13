using TMPro;
using UnityEngine;

public class ScrapMetalDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    private void Start()
    {
        EventManager.Instance.ScrapMetalChanged += OnChanged;
    }

    // TODO: animate, maybe
    private void OnChanged(int initialValue, int finalValue)
    {
        _label.text = finalValue.ToString();
    }
}
