public class CollectSeaweed : QuestStep
{
    private int _currentValue = 0;
    private readonly int _targetValue = 50;

    protected override string GetDescription()
    {
        return $"{_description} ({_currentValue} / {_targetValue})";
    }

    protected override void Start()
    {
        base.Start();
        EventManager.Instance.SeaweedChanged += OnGatherSeaweed;
    }

    private void OnGatherSeaweed(int initialValue, int finalValue)
    {
        _currentValue += finalValue - initialValue;
        if (_currentValue > _targetValue)
            _currentValue = _targetValue;
        UpdateDisplay();
        CheckCompletion();
    }

    protected override void CheckCompletion()
    {
        if (_currentValue >= _targetValue)
            Finish();
    }
}
