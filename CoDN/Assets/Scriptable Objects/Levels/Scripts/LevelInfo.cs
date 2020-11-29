using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que almacena la información de cada nivel
[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable/Level")]
public class LevelInfo : ScriptableObject
{
    [Header("Level")]
    [SerializeField] private List<Condition> conditions;
    [SerializeField] private Dialogue dialogueEN;
    [SerializeField] private Dialogue dialogueES;
    [Header("Available Taks")]
    [SerializeField] private List<GameObject> tasks;
    [Header("Cell")]
    [SerializeField] private int cellEnergy;
    [Header("ParticleSpawner")]
    [SerializeField] private List<GameObject> particles;
    [SerializeField] private List<int> particleWeights;
    
    public int CellEnergy { get => cellEnergy; set => cellEnergy = value; }
    public List<GameObject> Particles { get => particles; set => particles = value; }
    public List<int> ParticleWeights { get => particleWeights; set => particleWeights = value; }
    public Dialogue DialogueES { get => dialogueES; set => dialogueES = value; }
    public List<GameObject> Tasks { get => tasks; set => tasks = value; }
    public Dialogue DialogueEN { get => dialogueEN; set => dialogueEN = value; }

    public bool levelComplete = false;
    public bool levelEnd = false;

    //Devuelve true si el nivel debe terminar
    public bool checkConditions(CellInfo cellInfo)
    {
        if(conditions.Count > 0)
        {
            levelComplete = true;
            levelEnd = false;
            foreach (Condition c in conditions)
            {
                if (c.loose)
                {
                    levelEnd = c.Check(cellInfo);
                }
                else if (levelEnd == false)
                {
                    levelComplete = levelComplete && c.Check(cellInfo);
                }
                else
                {
                    levelComplete = false;                    
                }
            }
        }
        return levelEnd;
    }
}


