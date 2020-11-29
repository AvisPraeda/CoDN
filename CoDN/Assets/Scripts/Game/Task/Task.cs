using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase de las tareas de la célula
public class Task
{ 
    public TaskObject.TaskType type;
    public string info;
    
    public Task(TaskObject.TaskType t, string i = "0")
    {
        type = t;
        info = i;
    }
   
}
