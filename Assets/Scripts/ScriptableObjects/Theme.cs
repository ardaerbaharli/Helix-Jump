using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Theme", menuName = "Themes", order = 0)]
    public class Theme : ScriptableObject
    {
        public Material pole;
        public Material helix;
        public Material skybox;
    }
}