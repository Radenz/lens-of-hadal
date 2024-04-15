
using UnityEngine;

public class FollowPlayerBehavior : PathFindingBehavior
{
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Vector2 GetDestination()
    {
        return _player.position;
    }
}
