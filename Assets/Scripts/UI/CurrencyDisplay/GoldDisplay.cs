using TMPro;
using UnityEngine;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    private void Start()
    {
        EventManager.Instance.GoldChanged += OnChanged;
    }

    // TODO: animate, maybe
    private void OnChanged(int initialValue, int finalValue)
    {
        _label.text = finalValue.ToString();
    }
}
