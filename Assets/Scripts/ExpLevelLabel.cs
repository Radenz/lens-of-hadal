using TMPro;
using UnityEngine;

public class ExpLevelLabel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private TextMeshProUGUI _textOutline;

    private void Start()
    {
        EventManager.Instance.LevelledUp += OnLevelUp;
    }

    private void OnLevelUp(int level)
    {
        _text.text = level.ToString();
        _textOutline.text = level.ToString();
    }
}
