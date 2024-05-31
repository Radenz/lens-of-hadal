using TMPro;
using UnityEngine;

public class EnergyPowderDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private bool _isStaticDisplay = false;

    private void Start()
    {
        if (!_isStaticDisplay)
            EventManager.Instance.EnergyPowderChanged += OnChanged;
    }

    // TODO: animate, maybe
    private void OnChanged(int initialValue, int finalValue)
    {
        _label.text = finalValue.ToString();
    }
}
