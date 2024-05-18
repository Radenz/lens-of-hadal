using Common.Persistence;
using UnityEngine;

public class Bestiary : Singleton<Bestiary>, IBind<CreatureData>
{
    [SerializeField]
    private GameObject[] _pages;
    private int _currentPage;

    [SerializeField]
    private GameObject _nextPageButton;

    [SerializeField]
    private GameObject _prevPageButton;

    private bool HasPrevPage => _currentPage != 0;
    private bool HasNextPage => _currentPage * 2 + 2 < _pages.Length;

    private void Start()
    {
        foreach (GameObject page in _pages)
        {
            if (page) page.SetActive(false);
        }
        ShowPage(0);
    }

    public void Bind(CreatureData data)
    {
        foreach (GameObject page in _pages)
        {
            if (page.TryGetComponent(out BestiaryEntry entry))
            {
                entry.Bind(data);
            }
        }
    }

    public void PrevPage()
    {
        ShowPage(_currentPage - 1);
    }

    public void NextPage()
    {
        ShowPage(_currentPage + 1);
    }

    public void ShowPage(int pageNumber)
    {
        if (pageNumber * 2 >= _pages.Length) return;
        _currentPage = pageNumber;

        foreach (GameObject page in _pages)
        {
            if (page) page.SetActive(false);
        }

        _pages[pageNumber * 2]?.SetActive(true);
        if (pageNumber * 2 + 1 < _pages.Length)
            _pages[pageNumber * 2 + 1]?.SetActive(true);

        _prevPageButton.SetActive(HasPrevPage);
        _nextPageButton.SetActive(HasNextPage);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
