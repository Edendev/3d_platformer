using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Category/LevelSettings", order = 1)]
    public class LevelSettingsSO : ScriptableObject
    {
        public static uint DefaultUpdatablesCapacity = 100;

        public uint LevelID => levelId;
        public string LevelName => levelName;
        public int SceneBuildIndex => sceneBuildIndex;
        public Vector3 PlayerStartPosition => playerStartPosition;
        public Vector3 CameraUIPosition => cameraUIPosition;
        public Vector3 CameraUIRotation => cameraUIRotation;
        public uint InitialUpdatablesCapacity => initialUpdatablesCapacity;

        [SerializeField] private uint levelId;
        [SerializeField] private string levelName;
        [SerializeField] private int sceneBuildIndex;
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private Vector3 cameraUIPosition;
        [SerializeField] private Vector3 cameraUIRotation;
        [SerializeField] private uint initialUpdatablesCapacity = 100;
    }
}
