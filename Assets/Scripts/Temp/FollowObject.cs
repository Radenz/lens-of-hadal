using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private Transform _objectToFollow;
    private Transform _transform;

    [SerializeField]
    private float _smoothingFactor = 0;

    [SerializeField]
    private bool _followX;
    [SerializeField]
    private bool _followY;
    [SerializeField]
    private bool _followZ;

    private void Start()
    {
        _transform = transform;
    }

    private void Update()
    {
        Vector3 position = _transform.position;

        if (_followX) position.x = Mathf.Lerp(position.x, _objectToFollow.position.x, 1 - _smoothingFactor);
        if (_followY) position.y = Mathf.Lerp(position.y, _objectToFollow.position.y, 1 - _smoothingFactor);
        if (_followZ) position.z = Mathf.Lerp(position.z, _objectToFollow.position.z, 1 - _smoothingFactor);

        _transform.position = position;
    }
}
