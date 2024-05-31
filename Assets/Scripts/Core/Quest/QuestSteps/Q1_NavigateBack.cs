using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Q1_NavigateBack : QuestStep
{
    private CinemachineVirtualCamera _playerVCam;
    private CinemachineVirtualCamera _gate1VCam;
    private CinemachineVirtualCamera _gate2VCam;
    private GameObject _gate1;
    private GameObject _gate2;


    [SerializeField]
    private List<RangeTrigger> _firstCheckpoints;
    [SerializeField]
    private RangeTrigger _secondCheckpoints;
    [SerializeField]
    private List<RangeTrigger> _lastCheckpoints;

    private bool _isReached = false;
    private int _index = 0;

    protected override void Start()
    {
        base.Start();
        _gate1 = GameObject.FindGameObjectWithTag("Gate1");
        _gate2 = GameObject.FindGameObjectWithTag("Gate2");

        DisplayFirstTarget();
    }

    private async void ShowGate1ClosingCutscene()
    {
        _gate1VCam = GameObject.FindGameObjectWithTag("Gate1VCam").GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera _playerVCam = GameObject.FindGameObjectWithTag("PlayerVCam").GetComponent<CinemachineVirtualCamera>();
        CameraManager.Instance.ChooseCamera(_gate1VCam);

        await Awaitable.WaitForSecondsAsync(2f);

        await _gate1.transform
             .DOLocalRotate(new Vector3(0f, 0f, -88f), 2f)
             .SetEase(Ease.InOutQuad)
             .AsyncWaitForCompletion();

        CameraManager.Instance.ChooseCamera(_playerVCam);

        DisplaySecondTarget();
    }

    private async void ShowGate2ClosingCutscene()
    {
        _secondCheckpoints.gameObject.SetActive(false);
        _secondCheckpoints.Entered -= ShowGate2ClosingCutscene;

        _gate2VCam = GameObject.FindGameObjectWithTag("Gate2VCam").GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera _playerVCam = GameObject.FindGameObjectWithTag("PlayerVCam").GetComponent<CinemachineVirtualCamera>();
        CameraManager.Instance.ChooseCamera(_gate2VCam);

        await Awaitable.WaitForSecondsAsync(2f);

        await _gate2.transform
             .DOLocalRotate(new Vector3(0f, 0f, -103f), 2f)
             .SetEase(Ease.InOutQuad)
             .AsyncWaitForCompletion();

        CameraManager.Instance.ChooseCamera(_playerVCam);

        DisplayLastTarget();
    }

    protected override string GetDescription()
    {
        return "Navigate back to the ship";
    }

    protected override void CheckCompletion()
    {
        if (_isReached) Finish();
    }

    private void DisplayFirstTarget()
    {
        if (_index != 0)
        {
            RangeTrigger lastTrigger = _firstCheckpoints[_index - 1];
            lastTrigger.gameObject.SetActive(false);
        }

        if (_index >= _firstCheckpoints.Count)
        {
            _index = 0;
            ShowGate1ClosingCutscene();
            return;
        }

        RangeTrigger trigger = _firstCheckpoints[_index];
        trigger.gameObject.SetActive(true);
        trigger.Entered += DisplayFirstTarget;

        _index++;
    }

    private void DisplaySecondTarget()
    {
        _secondCheckpoints.gameObject.SetActive(true);
        _secondCheckpoints.Entered += ShowGate2ClosingCutscene;
    }

    private void DisplayLastTarget()
    {
        if (_index != 0)
        {
            RangeTrigger lastTrigger = _lastCheckpoints[_index - 1];
            lastTrigger.gameObject.SetActive(false);
        }

        if (_index >= _lastCheckpoints.Count)
        {
            _index = 0;
            _isReached = true;
            CheckCompletion();
            return;
        }

        RangeTrigger trigger = _lastCheckpoints[_index];
        trigger.gameObject.SetActive(true);
        trigger.Entered += DisplayLastTarget;

        _index++;
    }
}
