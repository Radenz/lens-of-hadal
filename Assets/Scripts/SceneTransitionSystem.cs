using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionSystem : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Quest", LoadSceneMode.Additive);
    }
}