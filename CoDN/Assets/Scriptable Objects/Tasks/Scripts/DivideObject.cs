using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Divide Object", menuName = "Scriptable/Tasks/Divide")]
public class DivideObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.divide;
    }
}
