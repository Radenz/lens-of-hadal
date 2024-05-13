using TMPro;
using UnityEngine;

public class SeaweedDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _label;

    private void Start()
    {
        EventManager.Instance.SeaweedChanged += OnChanged;
    }

    // TODO: animate, maybe
    private void OnChanged(int initialValue, int finalValue)
    {
        _label.text = finalValue.ToString();
    }
}
