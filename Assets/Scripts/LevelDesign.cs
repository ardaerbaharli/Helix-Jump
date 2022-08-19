using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelDesign : MonoBehaviour
{
    [SerializeField] private int changeThemeEveryLevels;

    [SerializeField] private bool overrideTheme;
    [SerializeField] private int overrideThemeIndex;

    [SerializeField] private Material pole;
    [SerializeField] private Material helix;
    [SerializeField] private Material skybox;


    private Queue<Theme> themes;
    private Theme currentTheme;
    public static LevelDesign instance;

    private void Awake()
    {
        instance = this;

        themes = Resources.LoadAll<Theme>("Themes").ToList().OrderBy(x => Random.value).ToQueue();

        if (overrideTheme)
        {
            var themeIndex = overrideThemeIndex;
            currentTheme = Resources.Load<Theme>($"Themes/Theme+{themeIndex}");
            themes.Dequeue(currentTheme);
        }
        else
            currentTheme = themes.Dequeue();

        themes.Enqueue(currentTheme);
        StartCoroutine(LoadCurrentTheme());
    }


    private void Start()
    {
        ScoreManager.instance.OnScored += OnScored;
    }

    private void OnScored(int score)
    {
        if (score % changeThemeEveryLevels == 0)
            FadeToNextTheme();
    }

    private IEnumerator LoadCurrentTheme()
    {
        var time = 1f;
        var deltaTime = 0f;
        while (deltaTime < 1)
        {
            deltaTime += Time.deltaTime / time;
            pole.Lerp(pole, currentTheme.pole, deltaTime);
            helix.Lerp(helix, currentTheme.helix, deltaTime);
            skybox.Lerp(skybox, currentTheme.skybox, deltaTime);
            yield return null;
        }
    }

    public void FadeToNextTheme()
    {
        currentTheme = themes.Dequeue();
        themes.Enqueue(currentTheme);
        StartCoroutine(LoadCurrentTheme());
    }
}