using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de las condiciones de victoria y derrota del nivel
[CreateAssetMenu(fileName = "New Condition", menuName = "Scriptable/Condition")]
public class Condition : ScriptableObject
{
    public enum conditionType
    {
        energy,
        size,
        task,
        codeSize
    }

    [SerializeField] public bool loose;
    [SerializeField] public conditionType type;
    [SerializeField] public TaskObject.TaskType taskType;
    [SerializeField] public int desiredValue;

    //Comprueba si la condición se ha cumplido a partir de la información de la célula
    public bool Check(CellInfo cellInfo)
    {
        switch (type)
        {
            case conditionType.energy:
                if (cellInfo.EnergyValue == desiredValue)
                {
                    return true;
                }
                return false;

            case conditionType.size:
                if (cellInfo.SizeValue == desiredValue)
                {
                    return true;
                }
                return false;

            case conditionType.codeSize:
                if(cellInfo.CodeSize >= desiredValue)
                {
                    return true;
                }
                return false;

            case conditionType.task:
                if(taskType == TaskObject.TaskType.any && cellInfo.ExecutedTasks.Count == desiredValue)
                {
                    return true;
                }
                else if (cellInfo.CountTask(taskType) >= desiredValue)
                {
                    return true;
                }
                return false;
        }
        return false;
    }

}
