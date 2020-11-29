using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase que almacena y actualiza la información de entrada de una tarea
public class InputParameter : MonoBehaviour
{
    [SerializeField] private CodeObject code;

    [SerializeField] private string info;
    [SerializeField] private Text line;
    [SerializeField] private Text type;
    [SerializeField] private InputField input;
    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;

    void Start()
    {
        SetLineInfo();
    }

    public void SetLineInfo()
    {
        
        TaskSlot taskSlot = code.Container[int.Parse(line.text)];
        if (input.text != "")
        {
            checkInput();
            info = input.text;
            Debug.Log("info = " + info);
            taskSlot.task.Info = info;
        } else
        {
            input.text = taskSlot.task.Info;
        }
    }

    public void checkInput()
    {
        int val = int.Parse(input.text);
        val = Mathf.Max(minValue,(Mathf.Min(maxValue, val)));
        input.text = val.ToString();
    }
}
