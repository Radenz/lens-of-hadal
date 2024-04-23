using UnityEngine;
using UnityEngine.UI;

public class DashButton : MonoBehaviour
{
    [SerializeField]
    private Image _button;

    private void Start()
    {
        Movement.Instance.Dashing += OnDash;
        Movement.Instance.DashCooldownFinished += OnCanDash;
    }

    private void OnDash()
    {
        SetAlpha(0.2f);
    }

    private void OnCanDash()
    {
        SetAlpha(1);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _button.color;
        color.a = alpha;
        _button.color = color;
    }
}
