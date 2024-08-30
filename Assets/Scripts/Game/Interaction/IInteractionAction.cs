using System;
using UnityEngine;

namespace Game.Transformables
{
    public interface IInteractionAction
    {
        void Perform(GameObject interactable, GameObject interactor);
    }
}
