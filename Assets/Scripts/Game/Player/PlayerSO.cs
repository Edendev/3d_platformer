using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Category/Player", order = 1)]
    public class PlayerSO : ScriptableObject
    {
        public PlayerController Player => player;
        public float WalkingSpeed => walkingSpeed;
        public float RotationSpeed => rotationSpeed;

        [SerializeField] private PlayerController player;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float rotationSpeed;
    }
}
