using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Clase que controla la funcionalidad del menú principal
public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator levelSelectAnimator;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
            Debug.Log("Unity Editor");
        #endif
        Time.timeScale = 1;
        //Inicia el audioManager y reproduce el tema principal
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.Play("MainTheme");
    }

    //Inicia la pantalla de juego
    public void LoadLevelScene()
    {
        ButtonPress();
        SceneManager.LoadScene("LevelScene");
    }

    //Abre el selector de niveles
    public void OpenLevelSelector()
    {
        levelSelectAnimator.SetBool("isOpen", true);
        ButtonPress();
    }

    //Cierra el selector de niveles
    public void CloseLevelSelector()
    {
        levelSelectAnimator.SetBool("isOpen", false);
        ButtonPress();
    }

    //Si el selector de niveles estaba abierto lo cierra y viceversa
    public void AlterLevelSelector()
    {
        bool open = levelSelectAnimator.GetBool("isOpen");
        levelSelectAnimator.SetBool("isOpen", !open);
    }

    //Funcionalidad
    public void Play()
    {
        OpenLevelSelector();
    }

    public void Options()
    {
        ButtonPress();
    }

    //Cierra la aplicación
    public void Exit()
    {
        Application.Quit();
    }

    //Reproduce un sonido al pulsar un botón
    public void ButtonPress()
    {
        audioManager.Play("ButtonPress");      
    }
}
