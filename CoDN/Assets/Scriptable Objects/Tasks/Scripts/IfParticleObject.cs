using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New If Particle Object", menuName = "Scriptable/Tasks/IfParticle")]
public class IfParticleObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.ifParticle;
    }
}

