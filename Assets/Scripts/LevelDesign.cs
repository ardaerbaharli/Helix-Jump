using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelDesign : MonoBehaviour
{
    [SerializeField] private int numberOfThemes;
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

    private Theme theme;
    public static LevelDesign instance;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        instance = this;

        var themeIndex = Random.Range(0, numberOfThemes);
        if (overrideTheme)
            themeIndex = overrideThemeIndex;
        var themeName = "Theme" + themeIndex;
        theme = Resources.Load<Theme>("Themes/" + themeName);

        ball.Lerp(ball, theme.ball, 1);
        ballTrail.Lerp(ballSuperSpeed, theme.ballTrail, 1);
        ballTrail.SetColor(EmissionColor, theme.ballTrail.GetColor(EmissionColor));
        ballTrail.EnableKeyword("_EMISSION");

        ballSuperSpeed.Lerp(ball, theme.ballSuperSpeed, 1);
        ballTrailSuperSpeed.Lerp(ballTrailSuperSpeed, theme.ballTrailSuperSpeed, 1);
        ballTrailSuperSpeed.SetColor(EmissionColor, theme.ballTrailSuperSpeed.GetColor(EmissionColor));
        ballTrailSuperSpeed.EnableKeyword("_EMISSION");

        pole.Lerp(pole, theme.pole, 1);
        helix.Lerp(helix, theme.helix, 1);
        skybox.Lerp(skybox, theme.skybox, 1);
        ballLight.color = theme.ballLightColor;
    }


    public object[] GetBallMaterials(bool isSuperSpeedOn)
    {
        return new object[]
        {
            isSuperSpeedOn ? ballSuperSpeed : ball,
            isSuperSpeedOn ? ballTrailSuperSpeed : ballTrail,
            isSuperSpeedOn ? theme.ballLightColorSuperSpeed : theme.ballLightColor
        };
    }
}