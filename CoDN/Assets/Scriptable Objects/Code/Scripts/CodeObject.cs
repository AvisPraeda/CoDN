using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Code", menuName = "Scriptable/Code")]

//Clase que contiene el código de la célula
public class CodeObject : ScriptableObject
{
    public List<TaskSlot> Container = new List<TaskSlot>();
    [SerializeField] private int maxTasks;
    private bool isUpdated;

    public int MaxTasks { get => maxTasks; set => maxTasks = value; }
    public bool IsUpdated { get => isUpdated; set => isUpdated = value; }

    //Añade una tarea al código
    public void AddTask(TaskObject _task)
    {
        if (Container.Count < MaxTasks)
        {
            Container.Add(new TaskSlot(_task, Container.Count));
        }
        Container.Sort((x, y) => x.line.CompareTo(y.line));
        isUpdated = false;
    }

    //Borra una tarea del código
    public void DeleteTask(TaskSlot _taskSlot)
    {
        if (Container.Contains(_taskSlot))
        {
            Container.Remove(_taskSlot);
        }
        Container.Sort((x, y) => x.line.CompareTo(y.line));
        isUpdated = false;
    }

    //Intercambia una tarea por otra
    public void MoveTask(TaskSlot _task1, TaskSlot _task2)
    {
        TaskSlot temp = new TaskSlot(_task2.task, _task2.line);
        _task2.UpdateTaskSlot(_task1.task, temp.line);
        _task1.UpdateTaskSlot(temp.task, _task1.line);
        isUpdated = false;
    }

    //Reordena las tareas tras mover una tarea a otra línea del código
    public void ReorderTasks(TaskSlot _task1, TaskSlot _task2)
    {
        ResetLines();
        int line1 = _task1.line;
        int line2 = _task2.line;
        /*Recorre las tareas que se encuentran entre la 
         * posición original y la posición a la que se
         * quiere mover la tarea y las desplaza hacia
         * arriba o hacia abajo dependiento si la línea
         destinada se encuentra antes o después*/
        if(line1 < line2)
        {
            for(int i=line1; i < line2; i++)
            {
                MoveTask(Container[i], Container[i + 1]);
            }
        } else
        {
            for(int i=line1; i > line2; i--)
            {
                MoveTask(Container[i], Container[i - 1]);
            }
        }
        Container.Sort((x,y)=>x.line.CompareTo(y.line));
    }

    public void DeleteCode()
    {
        Container.Clear();
        isUpdated = false;
    }
    
    public void ResetLines()
    {
        for(int i = 0; i < Container.Count; i++)
        {
            Container[i].line = i;
        }
    }
}
[System.Serializable]


//Clase que contiene la información necesaria para representar visualmente cada tarea en el código
public class TaskSlot
{
    public int line;
    public TaskObject task;

    public TaskSlot(TaskObject _task, int _line)
    {
        task = _task;
        line = _line;
    }

    public void UpdateTaskSlot(TaskObject _task, int _line)
    {
        task = _task;
        line = _line;
    }
}