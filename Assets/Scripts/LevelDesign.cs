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

    [SerializeField] private Material ball;
    [SerializeField] private Material ballTrail;
    [SerializeField] private Material ballSuperSpeed;
    [SerializeField] private Material ballTrailSuperSpeed;
    [SerializeField] private Light ballLight;

    [SerializeField] private Material pole;
    [SerializeField] private Material helix;
    [SerializeField] private Material skybox;

    private Queue<Theme> themes;
    private Theme currentTheme;
    public static LevelDesign instance;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

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
        var ballTrailTargetEmissionColor = currentTheme.ballTrail.GetColor(EmissionColor);
        var ballTrailSuperSpeedTargetEmissionColor = currentTheme.ballTrailSuperSpeed.GetColor(EmissionColor);
        var time = 1f;
        var deltaTime = 0f;
        while (deltaTime < 1)
        {
            deltaTime += Time.deltaTime / time;
            ball.Lerp(ball, currentTheme.ball, deltaTime);
            ballTrail.Lerp(ballSuperSpeed, currentTheme.ballTrail, deltaTime);

            ballTrail.EnableKeyword("_EMISSION");
            var ballTrailColor = Color.Lerp(ballTrail.GetColor(EmissionColor), ballTrailTargetEmissionColor, deltaTime);
            ballTrail.SetColor(EmissionColor, ballTrailColor);

            ballSuperSpeed.Lerp(ball, currentTheme.ballSuperSpeed, deltaTime);
            ballTrailSuperSpeed.Lerp(ballTrailSuperSpeed, currentTheme.ballTrailSuperSpeed, deltaTime);

            ballTrailSuperSpeed.EnableKeyword("_EMISSION");
            var ballTrailSuperSpeedColor = Color.Lerp(ballTrailSuperSpeed.GetColor(EmissionColor),
                ballTrailSuperSpeedTargetEmissionColor, deltaTime);
            ballTrailSuperSpeed.SetColor(EmissionColor, ballTrailSuperSpeedColor);

            pole.Lerp(pole, currentTheme.pole, deltaTime);
            helix.Lerp(helix, currentTheme.helix, deltaTime);
            skybox.Lerp(skybox, currentTheme.skybox, deltaTime);
            ballLight.color = Color.Lerp(ballLight.color, currentTheme.ballLightColor, deltaTime);
            yield return null;
        }
    }

    public void FadeToNextTheme()
    {
        currentTheme = themes.Dequeue();
        themes.Enqueue(currentTheme);
        StartCoroutine(LoadCurrentTheme());
    }


    public object[] GetBallMaterials(bool isSuperSpeedOn)
    {
        return new object[]
        {
            isSuperSpeedOn ? ballSuperSpeed : ball,
            isSuperSpeedOn ? ballTrailSuperSpeed : ballTrail,
            isSuperSpeedOn ? currentTheme.ballLightColorSuperSpeed : currentTheme.ballLightColor
        };
    }
}