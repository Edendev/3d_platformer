using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{   
    public interface ISystem
    {
        void Create();
        void Destroy();
    }
}
