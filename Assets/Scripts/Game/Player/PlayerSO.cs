using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Category/Player", order = 1)]
    public class PlayerSO : ScriptableObject
    {
        public GameObject PlayerGO => playerGO;
        public float WalkingSpeed => walkingSpeed;
        public float RotationSpeed => rotationSpeed;
        public float MaxJumpHeight => maxJumpHeight;
        public float MaxJumpSpeed => maxJumpSpeed;
        public AnimationCurve JumpSpeedCurve => jumpSpeedCurve;
        public float JumpMoveSpeedModifier => jumpMoveSpeedModifier;
        public float MinJumpTime => minJumpTime;
        public float DecelerationTime => decelerationTime;

        [SerializeField] private GameObject playerGO;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxJumpHeight;
        [SerializeField] private float maxJumpSpeed;
        [SerializeField] private AnimationCurve jumpSpeedCurve;
        [SerializeField, Range(0f, 1f)] private float jumpMoveSpeedModifier;
        [SerializeField] private float minJumpTime;
        [SerializeField] private float decelerationTime;
    }
}
