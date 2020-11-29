using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Extract Object", menuName = "Scriptable/Tasks/Extract")]
public class ExtractObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.extract;
    }
}
