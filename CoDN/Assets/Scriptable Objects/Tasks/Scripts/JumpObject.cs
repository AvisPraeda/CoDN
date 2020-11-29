using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Jump Object", menuName = "Scriptable/Tasks/Jump")]
public class JumpObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.jump;
    }
}
