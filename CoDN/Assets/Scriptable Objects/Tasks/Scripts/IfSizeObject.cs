using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New If Size Object", menuName = "Scriptable/Tasks/IfSize")]
public class IfSizeObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.ifSize;
    }
}
