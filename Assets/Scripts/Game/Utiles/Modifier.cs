using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utiles
{
    public class Modifier<T>
        where T : struct
    {
        public readonly object source;
        public readonly T value;
        private readonly Modification<T> modification;
        public Modifier(T value, Modification<T> modification, object source)
        {
            this.value = value;
            this.source = source;
            this.modification = modification;
        }

        public T Apply(T target)
        {
            return modification.Modify(target, value);
        }
    }
}
