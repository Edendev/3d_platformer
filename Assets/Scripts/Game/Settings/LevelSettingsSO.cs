using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Category/LevelSettings", order = 1)]
    public class LevelSettingsSO : ScriptableObject
    {
        public static uint DefaultUpdatablesCapacity = 100;

        public uint LevelID => levelId;
        public int SceneBuildIndex => sceneBuildIndex;
        public Vector3 PlayerStartPosition => playerStartPosition;
        public uint InitialUpdatablesCapacity => initialUpdatablesCapacity;

        [SerializeField] private uint levelId;
        [SerializeField] private int sceneBuildIndex;
        [SerializeField] private Vector3 playerStartPosition;
        [SerializeField] private uint initialUpdatablesCapacity = 100;
    }
}
