using System.Collections.Generic;
using System.Linq;
using Enums;
using Extensions;
using UnityEngine;
using Utilities;

public class PageController : MonoBehaviour
{
    private Pages _currentPage;
    private List<Pages> _previousPages;
    public DictionaryUnity<Pages, GameObject> pages;

    public Pages CurrentPage
    {
        get => _currentPage;
        set
        {
            _previousPages.Add(_currentPage);
            _currentPage = value;
        }
    }

    public List<Pages> PreviousPages => _previousPages;

    public static PageController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _currentPage = Pages.Loading;
    }

    private void Start()
    {
        _previousPages = new List<Pages>();
    }


    public void ShowPage(Pages page)
    {
        HidePage(_currentPage);
        _currentPage = page;
        pages[page].SetActive(true);
    }

    private void HidePage(Pages page)
    {
        PreviousPages.Add(page);
        pages[page].SetActive(false);
    }

    public void GoBack()
    {
        var page = PreviousPages.Last();
        PreviousPages.RemoveLast();

        pages[_currentPage].SetActive(false);
        _currentPage = page;
        pages[page].SetActive(true);
    }
}