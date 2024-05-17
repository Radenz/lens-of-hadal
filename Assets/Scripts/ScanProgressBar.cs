using UnityEngine;

public class ScanProgressBar : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _glowParticles;
    [SerializeField]
    private Transform _textTransform;

    public float YOffset;

    private static Color _startColor = new(1, 0, 0, 0.25f);
    private static Color _endColor = new(0, 1, 0, 0.25f);

    private void Start()
    {
        Vector3 position = _textTransform.localPosition;
        position.y = YOffset;
        _textTransform.localPosition = position;
    }

    public void SetProgress(float progress)
    {
        var module = _glowParticles.main;
        module.startColor = Color.Lerp(_startColor, _endColor, progress);
    }
}
