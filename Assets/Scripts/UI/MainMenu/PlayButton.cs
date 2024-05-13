using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image _scrim;

    public void OnPointerClick(PointerEventData eventData)
    {
        Play();
    }

    private async void Play()
    {
        await Task.Yield();
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync("World");
        loadSceneOperation.allowSceneActivation = false;
        await _scrim.DOFade(1f, 0.3f).AsyncWaitForCompletion();
        loadSceneOperation.allowSceneActivation = true;
    }
}
