using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private bool isPaused = true;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LevelSelector levelSelector;
    [SerializeField] private LevelInfo level;
    [SerializeField] private Vector2 worldLimit;
    [SerializeField] private float timeScale = 1;
    [SerializeField] private Habitat environment;
    [SerializeField] private ParticleSpawner particleSpawner;
    [SerializeField] private GameObject TaskContent;

    
    [Header("Cell")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private List<Cell> cells;
    [SerializeField] private TaskSystem taskSystem;
    private int processCount = 1;
    private int processSounds = 5;
    private int cellCount = 0;
    private CellInfo levelCellInfo;
    
    [Header("UI")]
    [SerializeField] private CodeDisplay codeDisplay;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private DebugManager debugManager;
    [SerializeField] private Color debugColor;
    [SerializeField] private PlayButton playButton;
    

    public TaskSystem TaskSystem { get => taskSystem; set => taskSystem = value; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public DebugManager DebugManager { get => debugManager; set => debugManager = value; }
    public AudioManager AudioManager { get => audioManager; set => audioManager = value; }
    public Vector2 WorldLimit { get => worldLimit; set => worldLimit = value; }

    void Start()
    {
        Debug.Log("GameHandler.Start");
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        initGameState();        
    }

    
    void Update()
    {

        if (!isPaused)
        {
            bool allCellsDead = true;

            foreach (Cell cell in cells)
            {
                if (cell != null && cell.Energy > 0)
                {
                    allCellsDead = false;
                }
            }

            if (allCellsDead)
            {
                SetPause(true);
                ResetGameState();
                debugManager.LogTextColor("Las células se han quedado sin energía", debugColor);
                Debug.Log("Cells died");
            }

            if (cells.Count > 0)
            {
                checkLevelComplete();
            }
        } 

    }

    /*Comprueba todos los objetivos del nivel para saber si 
     * el nivel se ha completado.*/
    private void checkLevelComplete()
    {
        bool levelCompleted = false;
        levelCellInfo = new CellInfo(0);
        /*Se comprueban los objetivos teniendo en cuenta la información del 
         * conjunto de células del nivel*/
        foreach (Cell cell in cells)
        {
            CellInfo cellInfo = cell.cellInfo;
            levelCellInfo.UpdateEnergy(cellInfo.EnergyValue);
            levelCellInfo.UpdateSize(cellInfo.SizeValue);            
            levelCellInfo.ExecutedTasks.AddRange(cellInfo.ExecutedTasks);
            /*En caso de que se cumpla alguna condición de derrota el juego 
             * se reinicia automáticamente*/
            if (level.checkConditions(levelCellInfo))
            {
                SetPause(true);
                ResetGameState();
                debugManager.LogTextColor("No se han cumplido los objetivos", debugColor);
            }
            /*En caso de que se cumplan todos los objetivos de victoria se 
             * establece que el nivel se ha completado*/
            else if(level.levelComplete)
            {
                levelCompleted = true;
            }
        }
        /*Si el nivel se ha completado se detiene la simulación, se muestran 
         * los resultados y se desbloquea el siguiente nivel*/
        if (levelCompleted)
        {
            SetPause(true);
            scoreManager.ShowScore(
                cells[0].cellInfo.CodeSize, levelCellInfo.TotalExecutedTasks(), this);
            UnlockNextLevel();
            debugManager.LogTextColor("Nivel completado", debugColor);
        }       
    }

    //Devuelve la posición de la célula indicada.
    public Vector3 GetCellPosition(int id)
    {
        return cells[id].Position;
    }

    //Establece el tasksystem para todas las células
    public void SetTaskSystem(TaskSystem t)
    {
        taskSystem = t;
        taskSystem.CodeDisplay = codeDisplay;
        foreach (Cell cell in cells)
        {
            TaskSystem tS = new TaskSystem(taskSystem);
            cell.SetTaskSystem(tS);
        }
        debugManager.LogTextColor("Inicio del código", debugColor);
    }

    //Establece el tasksystem para la célula indicada
    public void SetCellTaskSystem(Cell cell, TaskSystem t)
    {
        taskSystem = t;
        taskSystem.CodeDisplay = codeDisplay;
        TaskSystem tS = new TaskSystem(taskSystem);
        cell.SetTaskSystem(tS);
    }

    //Pausa o reanuda el juego
    public void SetPause(bool b)
    {
        isPaused = b;
        particleSpawner.IsPaused = b;
        playButton.UpdateSprite();
        if (!b)
        {
            SetTimeScale(timeScale);
            cells[0].Select();
        } 
    }

    //Establece la escala de tiempo del juego
    public void SetTimeScale(float f)
    {
        Time.timeScale = f;
    }

    //Instancia las tareas disponibles en el nivel actual
    public void SetTaskContent()
    {
        foreach(GameObject taskButton in level.Tasks)
        {
            Instantiate(taskButton, TaskContent.transform);
        }
    }

    //Limpia el contenido de la lista de tareas
    public void ClearTaskContent()
    {
        int childs = TaskContent.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            DestroyImmediate(TaskContent.transform.GetChild(i).gameObject);
        }
    }

    //Inicia el estado de juego
    public void initGameState()
    {
        //Obtiene información del nivel
        level = levelSelector.GetLevelInfo();
        //Establece el conjunto de tareas disponibles      
        ClearTaskContent();
        codeDisplay.Code.DeleteCode();
        SetTaskContent();
        //Añade la primera célula
        AddCell(level.CellEnergy);
        //Ajusta la aparición de partículas
        particleSpawner.Particles = level.Particles;
        particleSpawner.ChoiceWeights = level.ParticleWeights;
        //Inicia el diálogo
        Debug.Log("Starting Dialogue: " + level.DialogueES);
        switch (I18n.lang)
        {
            case "en":
                dialogueManager.StartDialogue(level.DialogueEN);
                break;
            case "es":
                dialogueManager.StartDialogue(level.DialogueES);
                break;
        }
    }

    //Reinicia el estado de juego
    public void ResetGameState()
    {
        ClearGameState();
        AddCell(level.CellEnergy);
    }

    //Limpia el estado de juego
    public void ClearGameState()
    {
        environment.RemoveAll();
        int temp = cells.Count;
        try
        {
            for (int i = temp - 1; i >= 0; i--)
            {
                if (cells[i] != null)
                {
                    RemoveCell(cells[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("GameHandler.ClearGameState Index out of bounds");
        }
        cells.Clear();
    }

    //Añade una célula al mundo con la energía y la posición indicadas
    public void AddCell(int energy, Vector2 position = default)
    {
        Vector2 randomPosition = new Vector2(
            UnityEngine.Random.Range(-0.5f, 0.5f), 
            UnityEngine.Random.Range(-0.5f, 0.5f));
        position += randomPosition;

        Quaternion rotation = 
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));

        GameObject newCellObject = 
            Instantiate(cellPrefab, position, Quaternion.identity, transform);       
        newCellObject.AddComponent<Cell>();

        Cell cell = newCellObject.GetComponent<Cell>();
        cell.Energy = energy;
        cell.GameHandler = this;
        cell.Id = cellCount;
        cellCount++;
        cells.Add(cell);

        if (taskSystem != null && !isPaused)
        {
            SetCellTaskSystem(cell, taskSystem);
        }
        audioManager.Play("PopSound");
    }

    //Elimina la célula indicada
    public void RemoveCell(Cell cell)
    {
        if(cell != null)
        {
            int id = cells.IndexOf(cell);
            cell.RemoveAll();
            Destroy(cell.gameObject);
            cells.RemoveAt(id);
        }
    }

    //Desbloquea el siguinte nivel
    public void UnlockNextLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(GameUtility.selectedLevelKey);
        int unlockedLevels = PlayerPrefs.GetInt(GameUtility.unlockedLevelsKey);
        if (currentLevel >= unlockedLevels)
        {
            PlayerPrefs.SetInt(GameUtility.unlockedLevelsKey, currentLevel);
        }
    }

    //Reproduce el sonido de respiración correspondiente
    public void PlayProccessSound()
    {
        processCount = (processCount % processSounds) + 1;
        switch (processCount)
        {
            case 1:
                audioManager.Play("Process1");
                break;
            case 2:
                audioManager.Play("Process2");
                break;
            case 3:
                audioManager.Play("Process3");
                break;
            case 4:
                audioManager.Play("Process4");
                break;
            case 5:
                audioManager.Play("Process5");
                break;
        }
    }
}
