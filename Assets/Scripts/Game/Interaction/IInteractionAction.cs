using System;
using UnityEngine;

namespace Game.Transformables
{
    public interface IInteractionAction
    {
        void Initialize();
        void Perform(GameObject interactable, GameObject interactor);
        void Reset(GameObject interactable);
    }
}
