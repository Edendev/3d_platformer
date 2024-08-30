using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Interfaces;

namespace Game.Utiles
{    public class ModifiableParameter<T> : IResetteable
        where T : struct
    {
        private T defaultValue;
        private List<Modifier<T>> modifiers = new List<Modifier<T>>();

        public ModifiableParameter(T defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public T Get()
        {
            T value = defaultValue;
            foreach (Modifier<T> modifier in modifiers) {
                value = modifier.Apply(value);
            }
            return value;
        }

        public void AddModifier(Modifier<T> modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifierFromSource(object source)
        {
            modifiers = modifiers.Where(x => x.source != source).ToList();
        }

        public void Reset() => modifiers.Clear();
    }
}

