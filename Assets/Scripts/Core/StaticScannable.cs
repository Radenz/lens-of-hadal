using NaughtyAttributes;
using UnityEngine;

public class StaticScannable : Scannable
{
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private int _expReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _energyPowderReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _seaweedReward;
    [SerializeField, MinMaxSlider(0, 100)]
    private Vector2Int _scrapMetalReward;

    [SerializeField]
    private float _regenDuration = 60f;

    public override void FinishScan()
    {
        IsScanned = true;
        _scanProgressBar.HideText();

        int energyPowder = _energyPowderReward.PickBetween();
        int seaweed = _seaweedReward.PickBetween();
        int scrapMetal = _scrapMetalReward.PickBetween();

        Announcer.Instance.AnnounceObjectScan(_sprite, energyPowder, seaweed, scrapMetal);
        EventManager.Instance.RewardPlayer(energyPowder, seaweed, scrapMetal);
        LevelManager.Instance.AddExp(_expReward);

        Timer.Instance.SetTimer(Regenerate, _regenDuration);
    }

    private void Regenerate()
    {
        Destroy(_scanProgressBarContainer);
        _scanProgressBarContainer = null;
    }
}
