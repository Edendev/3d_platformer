using System;
using UnityEngine;

namespace Game.Interaction
{
    public interface IInteractionAction
    {
        void Initialize(GameInstance gameInstance);
        void Perform(GameObject interactable, GameObject interactor);
        void Reset(GameObject interactable);
    }
}
