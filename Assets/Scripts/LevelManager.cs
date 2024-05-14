public class LevelManager : Singleton<LevelManager>
{
    private int _exp;
    private int _levelUpExp = 10;
    private int _level = 1;

    public int Level => _level;
    public int LevelUpExp => _levelUpExp;
    public int Exp => _exp;
    public float ExpPortion => (float)_exp / LevelUpExp;

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
            // TODO: change LevelUpExp
            LevelUp();
            return;
        }

        EventManager.Instance.ChangeExp(_exp);
    }

    private void LevelUp()
    {
        // TODO: invoke ChangeLevelUpExp
        EventManager.Instance.ChangeExp(_exp);
        EventManager.Instance.LevelUp(_level);
    }
}
