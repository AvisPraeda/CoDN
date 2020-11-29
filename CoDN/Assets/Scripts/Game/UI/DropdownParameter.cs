using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase que almacena y actualiza los valores de un parámetro desplegable de una tarea
public class DropdownParameter : MonoBehaviour
{
    [SerializeField] private CodeObject code;

    [SerializeField] private string info;
    [SerializeField] private Text line;
    [SerializeField] private Text type;
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private List<string> dropdownValues;

    void Start()
    {
        InitLineInfo();
    }

    public void InitLineInfo()
    {
        TaskSlot taskSlot = code.Container[int.Parse(line.text)];
        if (taskSlot.task.Info == "" || taskSlot.task.Info == null)
        {
            SetLineInfo();
        } else
        {
            dropdown.value = GetDropdownValue(taskSlot.task.Info);
        }
    }

    public void SetLineInfo()
    {
        TaskSlot taskSlot = code.Container[int.Parse(line.text)];
        info = dropdown.captionText.text;
        Debug.Log("info = " + info);
        taskSlot.task.Info = info;
    }

    public int GetDropdownValue(string info)
    {
        int value = dropdownValues.IndexOf(info);
        return value;
    }
}
