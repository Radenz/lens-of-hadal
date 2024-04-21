using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: refactor singletons
    public static PlayerController Instance;

    private Movement _movement;

    [SerializeField]
    private int _defaultHealthPoints;

    #region Player Attributes
    private int _healthPoints = 0;
    private int _dna = 0;
    private int _gold = 0;
    #endregion Player Attributes

    private void Awake()
    {
        Instance = this;
        _healthPoints = _defaultHealthPoints;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _movement.Bounce(direction, strength);
    }

    public void Damage()
    {
        // TODO: impl health system
    }
}
