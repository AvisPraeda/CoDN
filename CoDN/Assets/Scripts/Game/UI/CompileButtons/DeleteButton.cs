using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    [SerializeField] private CodeObject code;
    [SerializeField] private GameHandler gameHandler;
    private AudioManager audioManager;

    public void Start()
    {
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void DeleteCode()
    {
        DeleteSound();
        gameHandler.SetPause(true);
        TaskSystem t = new TaskSystem();
        code.DeleteCode();
        gameHandler.SetTaskSystem(t);
        gameHandler.ResetGameState();
    }

    public void DeleteSound()
    {
        if(audioManager != null)
        {
            audioManager.Play("DeleteCode");
        }
    }
}
