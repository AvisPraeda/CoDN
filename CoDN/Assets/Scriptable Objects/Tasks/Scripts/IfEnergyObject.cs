using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New If Energy Object", menuName = "Scriptable/Tasks/IfEnergy")]
public class IfEnergyObject : TaskObject
{
    public void Awake()
    {
        Type = TaskType.ifEnergy;
    }
}
