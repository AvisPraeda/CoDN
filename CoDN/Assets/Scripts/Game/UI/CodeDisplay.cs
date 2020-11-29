using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CodeDisplay : MonoBehaviour
{
    [SerializeField] private CodeObject code;
    private Dictionary<GameObject, TaskSlot> tasksDisplayed = new Dictionary<GameObject, TaskSlot>();   
    private MouseObject mouseObject = new MouseObject();
    private AudioManager audioManager;
    private bool insideCode;

    public CodeObject Code { get => code; set => code = value; }

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Comprueba si codeDisplay debe actualizarse
        if (!code.IsUpdated)
        {
            ClearDisplay();
            CreateDisplay();
        }     
    }

    //Crea la interfaz visual del código
    public void CreateDisplay()
    {
        //Recorre la lista de tareas del código
        for (int i = 0; i < code.Container.Count; i++)
        {
            //Crea un objeto a partir del Prefab correspondiente a la tarea
            var obj = Instantiate(code.Container[i].task.Prefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.SetSiblingIndex(i);
            obj.GetComponentInChildren<Text>().text = i.ToString();
            //Asigna eventos de interacción al objeto creado
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            tasksDisplayed.Add(obj, code.Container[i]);
        }
        code.IsUpdated = true;
    }

    //Actualiza la interfaz visual del código
    public void UpdateDisplay()
    {
        ClearDisplay();
        CreateDisplay();
        code.IsUpdated = true;
    }

    //Limpia la interfaz visual del código
    public void ClearDisplay()
    {
        foreach(KeyValuePair<GameObject, TaskSlot> kvp in tasksDisplayed)
        {
            Destroy(kvp.Key);
        }       
        tasksDisplayed.Clear();
    }

    //Resalta la línea indicada del código
    public void HighlightTask(int i)
    {
        foreach (KeyValuePair<GameObject, TaskSlot> kvp in tasksDisplayed)
        {
            
            if(kvp.Value.line == i)
            {
                Color c = kvp.Key.GetComponent<Image>().color;
                c.a = 1f;
                kvp.Key.GetComponent<Image>().color = c;
            }
            else
            {
                Color c = kvp.Key.GetComponent<Image>().color;
                c.a = 0.25f;
                kvp.Key.GetComponent<Image>().color = c;
            }
        }
    }

    //Restablece las líneas resaltadas
    public void removeHiglights()
    {
        foreach (KeyValuePair<GameObject, TaskSlot> kvp in tasksDisplayed)
        {

            Color c = kvp.Key.GetComponent<Image>().color;
            c.a = 0.25f;
            kvp.Key.GetComponent<Image>().color = c;
        }
    }

    //Borra el código al cerrar la aplicación
    private void OnApplicationQuit()
    {
        code.Container.Clear();
    }

    //Añade un activador de eventos a un objeto dados el tipo de evento y la acción de Unity que lo activa
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    private void OnEnter(GameObject obj)
    {
        mouseObject.hoverObj = obj;
        if (tasksDisplayed.ContainsKey(obj))
        {
            mouseObject.hoverTask = tasksDisplayed[obj];
        }  
    }

    private void OnExit(GameObject obj)
    {
        mouseObject.hoverObj = null;
        mouseObject.hoverTask = null;
        //insideCode = false;
    }

    //Crea una representación visual del objeto arrastrado
    private void OnBeginDrag(GameObject obj)
    {        
        GameObject newObj = Instantiate(obj, Vector3.zero, Quaternion.identity, transform.parent);
        newObj.GetComponent<Image>().raycastTarget = false;
        Destroy(newObj.GetComponent<Button>());
        Destroy(newObj.GetComponent<EventTrigger>());

        mouseObject.obj = newObj;
        mouseObject.task = tasksDisplayed[obj];
    }

    private void OnEndDrag(GameObject obj)
    {
        Destroy(mouseObject.obj);
        if (mouseObject.hoverObj != null)
        {
            code.ReorderTasks(tasksDisplayed[obj], tasksDisplayed[mouseObject.hoverObj]);
            ButtonPress();
        }
        else if (insideCode)
        {
            ButtonPress();
        }
        else
        {
            code.DeleteTask(mouseObject.task);
            DeleteSound();
        }       
        mouseObject.task = null;
    }

    private void OnDrag(GameObject obj)
    {
        if(mouseObject.obj != null)
        {
            mouseObject.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void ButtonPress()
    {
        if (audioManager != null)
        {
            audioManager.Play("ButtonPress");
        }
    }

    public void DeleteSound()
    {
        if (audioManager != null)
        {
            audioManager.Play("DeleteCode");
        }
    }

    public void SetInsideCode(bool b)
    {
        insideCode = b;
    }
}

//Clase que almacena la información del objeto arrastrado y el objeto sobre el que se encuentra el ratón
public class MouseObject
{
    public GameObject obj;
    public TaskSlot task;
    public GameObject hoverObj;
    public TaskSlot hoverTask;
}
