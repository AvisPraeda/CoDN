using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Destroy Object", menuName = "Scriptable/Tasks/Destroy")]
public class DestroyObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.destroyParticle;
    }
}
