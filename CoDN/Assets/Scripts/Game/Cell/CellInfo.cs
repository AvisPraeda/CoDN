using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo 
{
    private List<Task> executedTasks = new List<Task>();
    private int[] processedParticles = new int[3];
    private int codeSize;

    private float energyValue;
    private float minEnergyValue;
    private float maxEnergyValue;
    private float sizeValue;
    private float minSizeValue;
    private float maxSizeValue;

    public int CodeSize { get => codeSize; set => codeSize = value; }
    public float EnergyValue { get => energyValue; set => energyValue = value; }
    public float MinEnergyValue { get => minEnergyValue; set => minEnergyValue = value; }
    public float MaxEnergyValue { get => maxEnergyValue; set => maxEnergyValue = value; }
    public float SizeValue { get => sizeValue; set => sizeValue = value; }
    public float MinSizeValue { get => minSizeValue; set => minSizeValue = value; }
    public float MaxSizeValue { get => maxSizeValue; set => maxSizeValue = value; }
    public List<Task> ExecutedTasks { get => executedTasks; set => executedTasks = value; }
    public int[] ProcessedParticles { get => processedParticles; set => processedParticles = value; }

    public CellInfo(int codeSize)
    {
        CodeSize = codeSize;       
    }

    public void UpdateEnergy(float e)
    {
        energyValue = e;
        minEnergyValue = Mathf.Min(minEnergyValue, energyValue);
        maxEnergyValue = Mathf.Max(maxEnergyValue, energyValue);
    }

    public void UpdateSize(float s)
    {
        sizeValue = s;
        minSizeValue = Mathf.Min(minSizeValue, sizeValue);
        maxEnergyValue = Mathf.Max(maxSizeValue, sizeValue);
    }

    public void AddTask(Task task)
    {
        ExecutedTasks.Add(task);
    }

    public int CountTask(TaskObject.TaskType taskType)
    {
        int val = 0;
        foreach (Task task in ExecutedTasks)
        {
            if(task.type == taskType)
            {
                val++;
            }
        }
        return val;
    }

    
    public void processParticle(int type)
    {        
        ProcessedParticles[type]+=1;
        /* type:
        * 0 = positive
        * 1 = neutral
        * 2 = negative
        */
    }

    public int TotalExecutedTasks()
    {
        return ExecutedTasks.Count;
    }
}
