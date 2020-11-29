using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase encargada de comenzar la simulación del código
public class PlayButton : MonoBehaviour
{
    [SerializeField] private CodeObject code;
    [SerializeField] private GameHandler gameHandler;
    private TaskSystem taskSystem;
    [SerializeField] private Slider slider;
    [SerializeField] private Image image;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite stopSprite;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayCode()
    {
        ButtonPress();
        UpdateSprite();
        if (gameHandler.IsPaused)
        {        
            taskSystem = TaskSystemFromCode();
            gameHandler.SetTaskSystem(taskSystem);
            gameHandler.SetPause(false);
            gameHandler.SetTimeScale(slider.value);
        } else
        {
            gameHandler.SetPause(true);
            gameHandler.ResetGameState();
            gameHandler.SetTimeScale(1);
        }
    }

    //Crea el TaskSystem de las células a partir del CodeObject referenciado
    public TaskSystem TaskSystemFromCode()
    {
        TaskSystem TS = new TaskSystem();
        //Recorre el código genético y añade las tareas que debe realizar la célula
        foreach (TaskSlot t in code.Container)
        {
            string info = t.task.Info;
            Task task = new Task(t.task.Type, info);
            TS.addTask(task);
        }
        return TS;
    }

    //Actualiza el sprite del botón
    public void UpdateSprite()
    {
        if (gameHandler.IsPaused)
        {
            image.sprite = playSprite;
        }
        else
        {
            image.sprite = stopSprite;
        }
    }

    //Reproduce un sonido al presionar el botón
    public void ButtonPress()
    {
        if (audioManager != null)
        {
            audioManager.Play("ButtonPress");
        }
    }
}
