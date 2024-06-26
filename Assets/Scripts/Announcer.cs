using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Announcer : Singleton<Announcer>
{
    [Header("References")]
    [SerializeField]
    private GameObject _announcement;

    [SerializeField]
    private GameObject _scanAnnouncementPrefab;
    [SerializeField]
    private GameObject _objectScanAnnouncementPrefab;
    [SerializeField]
    private GameObject _discoveryAnnouncementPrefab;
    [SerializeField]
    private GameObject _rewardAnnouncementPrefab;

    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private GameObject _energyPowderRewardLabel;
    [SerializeField]
    private TextMeshProUGUI _energyPowderRewardLabelText;
    [SerializeField]
    private GameObject _seaweedRewardLabel;
    [SerializeField]
    private TextMeshProUGUI _seaweedRewardLabelText;
    [SerializeField]
    private GameObject _scrapMetalRewardLabel;
    [SerializeField]
    private TextMeshProUGUI _scrapMetalRewardLabelText;
    [SerializeField]
    private GameObject _goldRewardLabel;
    [SerializeField]
    private TextMeshProUGUI _goldRewardLabelText;

    [SerializeField]
    private float _duration = 2;
    public float Duration => _duration;

    private readonly List<AnnouncementT> _announcementQueue = new();
    private bool _isAnnouncing = false;

    private void Update()
    {
        if (!_isAnnouncing && _announcementQueue.Count != 0)
            AnnounceLeastRecent();
    }

    private void AnnounceLeastRecent()
    {
        AnnouncementT announcement = _announcementQueue[0];
        _announcementQueue.RemoveRange(0, 1);

        int unusedCurrencies = 0;
        List<GameObject> usedLabels = new();

        if (announcement.EnergyPowderReward == 0)
        {
            unusedCurrencies++;
            _energyPowderRewardLabel.SetActive(false);
        }
        else
        {
            usedLabels.Add(_energyPowderRewardLabel);
        }
        _energyPowderRewardLabelText.text = announcement.EnergyPowderReward.ToString();

        if (announcement.SeaweedReward == 0)
        {
            unusedCurrencies++;
            _seaweedRewardLabel.SetActive(false);
        }
        else
        {
            usedLabels.Add(_seaweedRewardLabel);
        }
        _seaweedRewardLabelText.text = announcement.SeaweedReward.ToString();

        if (announcement.ScrapMetalReward == 0)
        {
            unusedCurrencies++;
            _scrapMetalRewardLabel.SetActive(false);
        }
        else
        {
            usedLabels.Add(_scrapMetalRewardLabel);
        }
        _scrapMetalRewardLabelText.text = announcement.ScrapMetalReward.ToString();

        if (announcement.GoldReward == 0)
        {
            unusedCurrencies++;
            _goldRewardLabel.SetActive(false);
        }
        else
        {
            usedLabels.Add(_goldRewardLabel);
        }
        _goldRewardLabelText.text = announcement.ScrapMetalReward.ToString();

        float offset = -140 * (usedLabels.Count - 1);

        foreach (GameObject label in usedLabels)
        {
            Vector3 position = label.transform.localPosition;
            position.x = offset;
            offset += 280;
            label.transform.localPosition = position;
        }

        _title.text = announcement.Title.ToString();
        _isAnnouncing = true;
        _announcement.SetActive(true);
        Timer.Instance.SetTimer(HideAnnouncement, _duration);
    }

    private void HideAnnouncement()
    {
        _isAnnouncing = false;
        _announcement.SetActive(false);
    }

    public void AnnounceScan(Creature creature, int energyPowder, int seaweed, int scrapMetal)
    {
        GameObject obj = Instantiate(_scanAnnouncementPrefab);
        ScanAnnouncement announcement = obj.GetComponent<ScanAnnouncement>();
        announcement.Creature = creature;
        announcement.EnergyPowder = energyPowder;
        announcement.Seaweed = seaweed;
        announcement.ScrapMetal = scrapMetal;
    }

    public void AnnounceObjectScan(Sprite sprite, int energyPowder, int seaweed, int scrapMetal)
    {
        GameObject obj = Instantiate(_objectScanAnnouncementPrefab);
        ObjectScanAnnouncement announcement = obj.GetComponent<ObjectScanAnnouncement>();
        announcement.Sprite = sprite;
        announcement.EnergyPowder = energyPowder;
        announcement.Seaweed = seaweed;
        announcement.ScrapMetal = scrapMetal;
    }

    public void AnnounceDiscovery(Creature creature)
    {
        GameObject obj = Instantiate(_discoveryAnnouncementPrefab);
        DiscoveryAnnouncement announcement = obj.GetComponent<DiscoveryAnnouncement>();
        announcement.Creature = creature;
    }

    public void AnnounceReward(Reward reward, string title)
    {
        GameObject obj = Instantiate(_rewardAnnouncementPrefab);
        RewardAnnouncement announcement = obj.GetComponent<RewardAnnouncement>();
        announcement.Gold = reward.Gold;
        announcement.Item = reward.Item;
        announcement.Title = title;
    }

    // public void AnnounceNewBodyPart(int dnaReward)
    // {
    //     _announcementQueue.Add(new()
    //     {
    //         Title = NEW_BODY_PART_TITLE,
    //         DnaReward = dnaReward
    //     });
    // }

    // public void AnnounceNewCreature(string creatureName, int dnaReward)
    // {
    //     _announcementQueue.Add(new()
    //     {
    //         Title = NEW_CREATURE_TITLE,
    //         DnaReward = dnaReward
    //     });
    // }

    // public void AnnounceScan(int dnaReward)
    // {
    //     _announcementQueue.Add(new()
    //     {
    //         Title = BODY_PART_TITLE,
    //         DnaReward = dnaReward
    //     });
    // }
}

struct AnnouncementT
{
    public string Title;
    public string Name;
    public int EnergyPowderReward;
    public int SeaweedReward;
    public int ScrapMetalReward;
    public int GoldReward;
}
