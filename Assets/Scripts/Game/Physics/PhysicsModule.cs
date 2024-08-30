using Game.Settings;
using Game.Utiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PhysicsSystem
{   
    public class PhysicsModule
    {
        public event Action onGrounded;

        public bool IsGrounded => isGrounded;
        private bool isGrounded = false;

        public Transform Ground => ground;
        private Transform ground;

        private readonly Transform transform;
        private readonly ModifiableParameter<float> gravityConstant;
        private readonly float maxGravitySpeed;
        private readonly int layerMask;

        private float currentGravitySpeed = 0f;

        public PhysicsModule(float maxGravitySpeed, float gravityConstant, Transform transform) {
            this.transform = transform;
            this.maxGravitySpeed = maxGravitySpeed;
            this.gravityConstant = new ModifiableParameter<float>(gravityConstant);
            this.layerMask = LayerMask.GetMask("Default");
        } 
        
        public void Update(float deltaTime)
        {
            bool wasGrounded = IsGrounded;

            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 0.5f, layerMask); 
            
            if (isGrounded) {
                ground = hit.transform;
                currentGravitySpeed = 0f;
                
                if (!wasGrounded) {
                    onGrounded?.Invoke();
                }
                
                return;
            }
            
            currentGravitySpeed = Mathf.Clamp(currentGravitySpeed + gravityConstant.Get() * deltaTime, maxGravitySpeed, 0f);
            transform.position = transform.position + Vector3.up * currentGravitySpeed * deltaTime;
        }    

        public void AddGravityModifier(Modifier<float> modifier) {
            gravityConstant.AddModifier(modifier);
        }

        public void RemoveGravityModifier(object source) {
            gravityConstant.RemoveModifierFromSource(source);
        }
    }
}

