using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Announcer : Singleton<Announcer>
{
    [SerializeField]
    private GameObject _announcement;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _dnaRewardLabel;

    [SerializeField]
    private float _duration = 2;
    public float Duration => _duration;

    private readonly List<Announcement> _announcementQueue = new();
    private bool _isAnnouncing = false;

    private static readonly string NEW_BODY_PART_TITLE = "NEW BODY PART DISCOVERED";
    private static readonly string NEW_CREATURE_TITLE = "NEW CREATURE DISCOVERED";
    private static readonly string BODY_PART_TITLE = "BODY PART SCANNED";

    private void Update()
    {
        if (!_isAnnouncing && _announcementQueue.Count != 0)
            AnnounceLeastRecent();
    }

    private void AnnounceLeastRecent()
    {
        Announcement announcement = _announcementQueue[0];
        _announcementQueue.RemoveRange(0, 1);

        _dnaRewardLabel.text = announcement.DnaReward.ToString();
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

    public void AnnounceNewBodyPart(int dnaReward)
    {
        _announcementQueue.Add(new()
        {
            Title = NEW_BODY_PART_TITLE,
            DnaReward = dnaReward
        });
    }

    public void AnnounceNewCreature(string creatureName, int dnaReward)
    {
        _announcementQueue.Add(new()
        {
            Title = NEW_CREATURE_TITLE,
            DnaReward = dnaReward
        });
    }

    public void AnnounceScan(int dnaReward)
    {
        _announcementQueue.Add(new()
        {
            Title = BODY_PART_TITLE,
            DnaReward = dnaReward
        });
    }
}

struct Announcement
{
    public string Title;
    public string Name;
    public int DnaReward;
}
