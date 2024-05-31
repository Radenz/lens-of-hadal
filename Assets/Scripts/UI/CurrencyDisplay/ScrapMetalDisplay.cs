using TMPro;
using UnityEngine;

public class ScrapMetalDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private bool _isStaticDisplay = false;

    private void Start()
    {
        if (!_isStaticDisplay)
            EventManager.Instance.ScrapMetalChanged += OnChanged;
    }

    // TODO: animate, maybe
    private void OnChanged(int initialValue, int finalValue)
    {
        _label.text = finalValue.ToString();
    }
}
