using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase abstracta para las tareas que almacena su información y el objeto prefabricado de dicha tarea
public abstract class TaskObject : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private TaskType type;
    [TextArea (15,20)]
    [SerializeField] private string description;

    [SerializeField] private string info;

    public enum TaskType
    {
        
        introduce,
        process,
        extract,      
        jump,
        ifParticle,
        ifEnergy,
        ifSize,
        divide,
        destroyParticle,
        any,
        color,
        none

        /*
         * Posible tasks: 
         * moveRandom,
         * stop
         */

    }

    public GameObject Prefab { get => prefab; set => prefab = value; }
    public TaskType Type { get => type; set => type = value; }
    public string Info { get => info; set => info = value; }
    public string Description { get => description; set => description = value; }
}
