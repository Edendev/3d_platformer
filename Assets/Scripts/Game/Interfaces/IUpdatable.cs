using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    int Hash { get; }
    void FrameUpdate(float deltaTime);
}
