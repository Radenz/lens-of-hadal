using UnityEngine;

public class SonarManager : MonoBehaviour
{
    [SerializeField]
    private float _pingDelay;

    private void Start()
    {
        Timer.Instance.SetTimer(Ping, _pingDelay);
    }

    private void Ping()
    {
        Debug.Log("Pinging Sonar");
        EventManager.Instance.PingSonar();
        Timer.Instance.SetTimer(Ping, _pingDelay);
    }
}
