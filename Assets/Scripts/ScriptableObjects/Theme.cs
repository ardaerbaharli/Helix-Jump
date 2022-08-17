using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Theme", menuName = "Themes", order = 0)]
    public class Theme : ScriptableObject
    {
        public Material ball;
        public Material ballTrail;
        public Material ballSuperSpeed;
        public Material ballTrailSuperSpeed;
        public Material pole;
        public Material helix;
        public Material skybox;
        public Color ballLightColor;
        public Color ballLightColorSuperSpeed;
    }
}