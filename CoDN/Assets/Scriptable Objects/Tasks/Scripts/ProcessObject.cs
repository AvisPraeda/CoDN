using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Process Object", menuName = "Scriptable/Tasks/Process")]
public class ProcessObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.process;
    }
}
