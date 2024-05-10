using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionSystem : MonoBehaviour
{
    [SerializeField]
    private Image _scrim;

    [SerializeField]
    private float _fadeDuration = 0.6f;

    private GameplayScene _currentScene = GameplayScene.World;

    private void Start()
    {
        SceneManager.LoadScene("Quest", LoadSceneMode.Additive);
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }

    public void OpenQuestMenu()
    {
        _currentScene = GameplayScene.Quest;

        DOTween.Sequence()
            .Join(_scrim.DOFade(1f, _fadeDuration / 2)
                .OnComplete(QuestMenu.Instance.Show)
            )
            .Append(_scrim.DOFade(0f, _fadeDuration / 2));
    }

    public void OpenShopMenu()
    {
        _currentScene = GameplayScene.Shop;

        DOTween.Sequence()
           .Join(_scrim.DOFade(1f, _fadeDuration / 2)
               .OnComplete(ShopMenu.Instance.Show)
           )
           .Append(_scrim.DOFade(0f, _fadeDuration / 2));
    }

    public void OpenWorld()
    {
        TweenCallback hideMenu = _currentScene == GameplayScene.Quest
            ? QuestMenu.Instance.Hide
            : ShopMenu.Instance.Hide;

        _currentScene = GameplayScene.World;

        DOTween.Sequence()
           .Join(_scrim.DOFade(1f, _fadeDuration / 2)
               .OnComplete(hideMenu)
           )
           .Append(_scrim.DOFade(0f, _fadeDuration / 2));
    }
}

public enum GameplayScene
{
    World,
    Quest,
    Shop,
}
