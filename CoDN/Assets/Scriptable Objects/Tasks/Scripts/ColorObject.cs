using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Color Object", menuName = "Scriptable/Tasks/Color")]
public class ColorObject : TaskObject
{
    private void Awake()
    {
        Type = TaskType.color;
    }
}
