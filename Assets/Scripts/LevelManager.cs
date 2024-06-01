using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private Reward[] _levelUpRewards;

    private int _exp;
    private int _levelUpExp = 50;
    private int _level = 1;

    public int Level => _level;
    public int LevelUpExp => _levelUpExp;
    public int Exp => _exp;
    public float ExpPortion => (float)_exp / LevelUpExp;

    private static int GetRequiredExp(int level)
    {
        return 25 + (level - 1) * 10;
    }


    private void Start()
    {
        // TODO: invoke
    }

    public void AddExp(int exp)
    {
        _exp += exp;
        if (_exp >= _levelUpExp)
        {
            _exp -= _levelUpExp;
            _level += 1;
            LevelUp();
            return;
        }

        EventManager.Instance.ChangeExp(_exp);
    }

    private void LevelUp()
    {
        _levelUpExp = GetRequiredExp(_level);
        EventManager.Instance.ChangeExp(_exp);
        EventManager.Instance.LevelUp(_level);

        Reward reward = _levelUpRewards.Length > _level - 2
            ? _levelUpRewards[_level - 2]
            : null;

        if (reward != null)
            reward.Give();
    }
}
