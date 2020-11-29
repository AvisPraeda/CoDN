using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Introduce Object", menuName = "Scriptable/Tasks/Introduce")]
public class IntroduceObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.introduce;
    }
}
