using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class used by the buttons that create tasks
public class TaskButton : MonoBehaviour
{
    [SerializeField] private TaskObject taskObject;
    [SerializeField] private Text buttonText;
    [SerializeField] private CodeObject code;
    private AudioManager audioManager;

    private void Start()
    {
        //buttonText.text = taskObject.Type.ToString();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void addTask()
    {
        taskObject = Instantiate(taskObject);
        code.AddTask(taskObject);
    }

    public void ButtonPress()
    {
        if(audioManager != null)
        {
            audioManager.Play("ButtonPress");
        }       
    }
}
