using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sistema mediante el cual la célula obtiene la tarea que debe realizar
public class TaskSystem
{
    [SerializeField] private CodeDisplay codeDisplay;
    private int taskCounter;
    public int TaskCounter { get => taskCounter; }

    private List<Task> taskList;

    public CodeDisplay CodeDisplay { get => codeDisplay; set => codeDisplay = value; }
    public List<Task> TaskList { get => taskList; set => taskList = value; }

    public TaskSystem()
    {
        TaskList = new List<Task>();
        taskCounter = 0;
    }

    public TaskSystem(TaskSystem ts)
    {
        TaskList = ts.TaskList;
        codeDisplay = ts.CodeDisplay;
        taskCounter = 0;
    }

    public void Copy(TaskSystem ts)
    {
        TaskList = ts.TaskList;
        codeDisplay = ts.CodeDisplay;
    }

    public Task getNextTask(bool show)
    {
        if (TaskList.Count > 0 && taskCounter < TaskList.Count)
        {
            Task task = TaskList[taskCounter];
            Debug.Log("Executing Task " + taskCounter);
            if(show) codeDisplay.HighlightTask(taskCounter);
            taskCounter++;
            return task;
        }
        else
        {
            return null;
        }
    }

    public void addTask(Task task)
    {
        TaskList.Add(task);
    }

    public void setNextTask(int n)
    {
        taskCounter = n;
    }

    public int taskListSize()
    {
        return TaskList.Count;
    }
}

