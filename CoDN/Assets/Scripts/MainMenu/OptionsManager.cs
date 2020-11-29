using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Clase que controla la funcionalidad del menú de opciones
public class OptionsManager : MonoBehaviour
{   
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Dropdown LangDropdown;
    [SerializeField] private List<string> dropdownValues;
    private bool reload;

    void Start()
    {
        InitLangInfo();
    }

    //Establece el idioma inicial del despegable
    public void InitLangInfo()
    {
        string lang = "Español";
        switch (PlayerPrefs.GetString("lang"))
        {
            case "en":
                lang = "English";
                break;
            case "es":
                lang = "Español";
                break;
        }
        LangDropdown.value = GetDropDownValue(lang);
    }

    //Muestra el menú de opciones
    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    //Esconde el menú de opciones
    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (reload)
        {
            SceneManager.LoadScene("MenuScene");
            reload = false;
        }     
    }

    //Establece el volumen de la música
    public void SetMusicVolume(float vol)
    {
        if (vol == -40) vol = -80;
        masterMixer.SetFloat("musicVolume", vol);
    }

    //Establece el volumen de los efectos de sonido
    public void SetSoundVolume(float vol)
    {
        if (vol == -40) vol = -80;
        masterMixer.SetFloat("soundVolume", vol);
    }

    //Establece el valor del desplegable de idioma
    public void SetDropdownValue()
    {
        string lang = PlayerPrefs.GetString("lang");
    }

    //Establece el idioma del juego
    public void SetGameLanguage()
    {
        string lang = LangDropdown.captionText.text;
        
        if (lang != null)
        {
            reload = true;
            switch (lang){
                case "English":
                    PlayerPrefs.SetString("lang", "en");
                    I18n.LoadLanguage();
                    break;
                case "Español":
                    PlayerPrefs.SetString("lang", "es");
                    I18n.LoadLanguage();
                    break;
            }
        }
    }

    public int GetDropDownValue(string lang)
    {
        int value = dropdownValues.IndexOf(lang);
        return value;
    }
}
