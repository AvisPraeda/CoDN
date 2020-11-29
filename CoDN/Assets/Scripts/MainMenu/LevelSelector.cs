using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase que controla el funcionamiento del selector de niveles
public class LevelSelector : MonoBehaviour
{
    [SerializeField] private List<LevelInfo> levels;
    [SerializeField] private List<Button> buttons;


    private void Start()
    {
        int unlockedLevels = PlayerPrefs.GetInt(GameUtility.unlockedLevelsKey);
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if(i > unlockedLevels)
                {
                    buttons[i].interactable = false;
                }
            }
        }
    }

    //Establece el nivel actual a partir del valor indicado como parámetro
    public void SelectLevel(int n)
    {
        PlayerPrefs.SetInt(GameUtility.selectedLevelKey, n);
    }

    //Devuelve la información del nivel actual
    public LevelInfo GetLevelInfo()
    {
        int level = PlayerPrefs.GetInt(GameUtility.selectedLevelKey);
        if (level < levels.Count)
        {
            return levels[level];
        } else
        {
            return levels[0];
        }      
    }

}
