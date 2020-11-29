using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //Parameters
    [SerializeField] private int id;
    [SerializeField] private bool isSelected;
    [SerializeField] private InfoBar energyBar;
    [SerializeField] private int size;
    [SerializeField] private int maxSize;
    [SerializeField] private int energy;
    [SerializeField] private int maxEnergy;

    [SerializeField] private Vector2 worldLimit;
    [SerializeField] private float speed = 5;
    [SerializeField] private Vector2 position;
    [SerializeField] private Vector2 scale;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private Habitat environment;
    [SerializeField] private TaskSystem taskSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    public CellInfo cellInfo;
   
    private GameHandler gameHandler;
    private bool isProcessing;

    public enum cellState
    {
        Waiting,
        Introducing,
        Extracting,
        Processing,
        Dividing,
        Destroying
    }

    [SerializeField] private cellState state;
    [SerializeField] private int storePosition;
    [SerializeField] private float storeOffsetX = -0.5f;
    [SerializeField] private float storeOffsetY = 0.33f;

    [Header("Particles")]
    [SerializeField] private List<Particle> particles;

    [Header("Time")]
    [SerializeField] private float timeProcess;
    [SerializeField] private float timeProcessLeft;
    [SerializeField] private float timeExtract;
    [SerializeField] private float timeExtractLeft;
    [SerializeField] private float timeDivision;
    [SerializeField] private float timeDivisionLeft;

    [Header("Sounds")]
    [SerializeField] private int maxParticleSounds = 15;
    [SerializeField] private int soundsPerParticle = 5;

    //Getters and Setters
    public GameHandler GameHandler { get => gameHandler; set => gameHandler = value; }
    public float TimeProcess { get { return timeProcess; } }
    public int Size { get => size; set => size = value; }
    public int MaxSize { get => maxSize; set => maxSize = value; }
    public int Energy { get => energy; set => energy = value; }
    public int MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
    public List<Particle> Particles { get => particles; set => particles = value; }
    public float Speed { get => speed; set => speed = value; }
    public Vector2 Position { get => position; set => position = value; }
    public Vector2 Scale { get => scale; set => scale = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public int Id { get => id; set => id = value; }
    

    void Start()
    {
        //Se obtienen parámentros del entorno y del propio objeto de la célula
        environment = GameObject.FindWithTag("Environment").GetComponent<Habitat>();
        energyBar = transform.Find("InfoBar").GetComponent<InfoBar>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particles = new List<Particle>();
        worldLimit = gameHandler.WorldLimit;
        position = transform.position;
        scale = transform.localScale;
        //Se establecen parámetros por defecto;
        state = cellState.Waiting;
        animator.SetInteger("CellState", 1);
        maxEnergy = 10;
        maxSize = 5;
        timeProcess = 2f;
        timeProcessLeft = timeProcess;
        timeExtract = 1f;
        timeExtractLeft = timeExtract;
        timeDivision = 1f;
        timeDivisionLeft = timeDivision;
    }

    
    void Update()
    {
        //Si la energía es menor o igual a cero se destruye la célula
        if(energy <= 0 && gameHandler != null)
        {
            this.Destroy();
            return;
        }
        //Dependiendo del estado de la célula se determina su siguiente acción
        switch (state)
        {
            //Solicita la siguiente tarea a realizar
            case cellState.Waiting:
                RequestNextTask();
                break;
            case cellState.Introducing:
                //Recorre las partículas que han entrado en contacto con la célula
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<Particle>())
                    {
                        Particle p = child.GetComponent<Particle>();
                        //Si encuentra alguna, la introduce
                        if (p.State == Particle.particleState.Contact)
                        {
                            IntroduceParticle(p);
                            break;
                        }
                    }
                }
                break;
            case cellState.Processing:
                //Recorre las partículas que se encuentran en el interior de la célula
                foreach (Particle p in particles)
                {
                    //Si encuentra una partícula en espera, comienza a procesarla
                    if(p.State == Particle.particleState.Waiting)
                    {                       
                        ProcessParticle(p);
                        break;
                    }
                }
                //Si no hay ninguna partícula en su interior pasa al estado de espera
                if(particles.Count == 0) state = cellState.Waiting;
                break;           
            case cellState.Extracting:
                //Recorre las partículas que se encuentran en el interior de la célula
                foreach (Particle p in particles)
                {
                    //Si encuentra alguna partícula la extrae
                    ExtractParticle(p);                                            
                    break;
                }
                //Si no hay ninguna partícula en su interior pasa al estado de espera
                if (particles.Count == 0) state = cellState.Waiting;
                break;
            case cellState.Dividing:
                //Comienza la división celular
                DivideCell();
                break;
            case cellState.Destroying:
                //Recorre las partículas que se encuentran en el interior de la célula
                foreach (Particle p in particles)
                {
                    //Si encuentra alguna partícula la destruye
                    DestroyParticle(p);
                    break;
                }
                //Si no hay ninguna partícula en su interior pasa al estado de espera
                if (particles.Count == 0) state = cellState.Waiting;
                break;
        }

        /*
        //Control del movimiento de la célula (Desactivado)
        if (isSelected) {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        //*/

        //Se actualizan los datos de la célula
        float energyValue = (float)energy / maxEnergy;
        energyBar.SetSize(energyValue);
        position = transform.position;
        scale = transform.localScale;
    }

    private void FixedUpdate()
    {
        position.x = rb.position.x + movement.x * speed * Time.fixedDeltaTime;
        position.y = rb.position.y + movement.y * speed * Time.fixedDeltaTime;
        //Ajusta la posición a los límites del mundo
        position.x = Mathf.Clamp(position.x, -worldLimit.x, worldLimit.x);
        position.y = Mathf.Clamp(position.y, -worldLimit.y, worldLimit.y);
        //*/
        rb.MovePosition(position);
        
    }

    //Establece el TaskSystem de la célula
    public void SetTaskSystem(TaskSystem t)
    {
        taskSystem = t;
        /*Genera cellInfo, la información con la que 
         * se comprueban los objetivos del nivel*/
        cellInfo = new CellInfo(taskSystem.taskListSize());
        cellInfo.UpdateEnergy(energy);
        cellInfo.UpdateSize(size);
    }
    //Solicita una nueva tarea de TaskSystem
    private void RequestNextTask()
    {
        if(taskSystem != null)
        {
            Task task = taskSystem.getNextTask(isSelected);
            //Si no hay tareas o se ha llegado al fin del código se reinicia el juego
            if (task == null)
            {
                if (isSelected) gameHandler.DebugManager.LogText(
                    "Fin del código de la célula número: " + Id);              
                gameHandler.ResetGameState();
                gameHandler.SetPause(true);
                Debug.Log("End of code");
            }
            //Se ejecuta la tarea
            else
            {
                ExecuteTask(task);               
            }
        }     
    }

    //Ejecuta la tarea indicada
    private void ExecuteTask(Task task)
    {
        
        switch (task.type)
        {
            //Pasa al estado Introducing
            case TaskObject.TaskType.introduce:
                state = cellState.Introducing;
                break;
            //Pasa al estado Processing
            case TaskObject.TaskType.process:
                state = cellState.Processing;
                break;
            //Pasa al estado Extracting
            case TaskObject.TaskType.extract:
                state = cellState.Extracting;
                break;
            //La siguiente tarea en ejecutarse en el código pasa a ser la marcada como información de salto
            case TaskObject.TaskType.jump:
                cellInfo.AddTask(task);
                int nextTask = int.Parse(task.info);
                if (isSelected) gameHandler.DebugManager.LogText("Salto a la tarea nº: " + nextTask);
                Debug.Log("Jump to Task: " + nextTask);
                taskSystem.setNextTask(nextTask);
                break;
            /*Si la primera partícula introducida coincide con 
             * el tipo de partícula almacenado en la información 
             * de la tarea, se ejecuta la siguiente instrucción y,
             en caso contrario se la salta*/
            case TaskObject.TaskType.ifParticle:
                cellInfo.AddTask(task);
                if (particles.Count > 0)
                {
                    if(particles[0].TypeToString() != task.info)
                    {
                        Debug.Log("False " + particles[0].TypeToString() + " != " + task.info);
                        taskSystem.setNextTask(taskSystem.TaskCounter+1);
                    } else
                    {
                        Debug.Log("True " + particles[0].TypeToString() + " == " + task.info);
                    }
                }
                break;
            /*Si el valor de reserva de la célula coincide con 
             * el valor de reserva almacenado en la información 
             * de la tarea, se ejecuta la siguiente instrucción y,
             en caso contrario se la salta*/
            case TaskObject.TaskType.ifSize:
                cellInfo.AddTask(task);
                if (size > int.Parse(task.info))
                {
                    Debug.Log("True, size > " + task.info);
                } else
                {
                    Debug.Log("False, size <= " + task.info);
                    taskSystem.setNextTask(taskSystem.TaskCounter + 1);
                }
                break;
            /*Si el valor de energía de la célula coincide con 
             * el valor de energía almacenado en la información 
             * de la tarea, se ejecuta la siguiente instrucción y,
             en caso contrario se la salta*/
            case TaskObject.TaskType.ifEnergy:
                cellInfo.AddTask(task);
                if (energy > int.Parse(task.info))
                {
                    Debug.Log("True, energy = " + energy + " > " + task.info);
                }
                else
                {
                    Debug.Log("False, energy = " + energy + " <= " + task.info);
                    taskSystem.setNextTask(taskSystem.TaskCounter + 1);
                }
                break;
            /*Si la célula posee energía suficiente para dividirse
             pasa al estado Dividing*/
            case TaskObject.TaskType.divide:
                if (energy > maxEnergy / 2)
                {
                    Debug.Log("Cell dividing");
                    state = cellState.Dividing;
                } else
                {
                    Debug.Log("Not enough energy to divide");
                }
                break;
            //Pasa al estado Destroying
            case TaskObject.TaskType.destroyParticle:
                state = cellState.Destroying;
                break;
            //Modifica el color de la célula por el valor de color almacenado en la información de la tarea
            case TaskObject.TaskType.color:
                cellInfo.AddTask(task);
                setColor(task.info);
                break;
        }
    }

    //Introduce una partícula en la célula si hay espacio suficiente
    void IntroduceParticle(Particle particle)
    {
        if (size  >= maxSize)
        {
            energy = 0;
            if (isSelected) gameHandler.DebugManager.LogText("No se pueden introducir más pratículas en la célula");
            Debug.Log("Can't introduce more particles in the Cell");          
        } else
        {          
            if (isSelected) gameHandler.DebugManager.LogText("Partícula introducida");
            if (environment.Size < size)
            {
                energy--;
            }

            particles.Add(particle);           
            Vector2 targetPosition = new Vector2();
            targetPosition.x += storeOffsetX * scale.x/0.5f;
            targetPosition.y += storePosition * storeOffsetY - 2*storeOffsetY * scale.y/0.5f;
            particle.SetTarget(targetPosition);           
            storePosition = (storePosition + 1) % maxSize;
            particle.SetState(Particle.particleState.Moving);
            particle.Inside = true;
            particle.UpdateColor(spriteRenderer.color);
            environment.removeParticle(particle);
            state = cellState.Waiting;
            size++;
            cellInfo.UpdateEnergy(energy);
            cellInfo.UpdateSize(size);
            cellInfo.AddTask(new Task(TaskObject.TaskType.introduce, "0"));
        }      
    }
    
    //Extrae una partícula de la célula
    void ExtractParticle(Particle particle)
    {
        if (particles.Contains(particle))
        {           
            particle.SetTarget((particle.Position-position)*15);
            particle.SetState(Particle.particleState.Moving);        
            timeExtractLeft -= Time.deltaTime;
            if (timeExtractLeft <= 0)
            {
                if (isSelected) gameHandler.DebugManager.LogText("Partícula extraída");
                if (particle.Type == Particle.particleType.Negative)
                {
                    energy-= 2;
                } else if(environment.Size > size)
                {
                    energy--;
                }

                environment.addParticle(particle);
                particles.Remove(particle);
                particle.transform.SetParent(environment.transform);
                particle.Inside = false;
                state = cellState.Waiting;
                size--;
                cellInfo.UpdateEnergy(energy);
                cellInfo.UpdateSize(size);
                cellInfo.AddTask(new Task(TaskObject.TaskType.extract, "0"));
                timeExtractLeft = timeExtract;
            }          
        } else
        {
            if (isSelected) gameHandler.DebugManager.LogText("No se ha podido extraer ninguna partícula");
            Debug.Log("Particle not found at Cell.extractParticle()");
        }
    }
    
    //Procesa una partícula de la célula
    public void ProcessParticle(Particle particle)
    {
        if (particles.Contains(particle))
        {
            timeProcessLeft -= Time.deltaTime;
            if (!isProcessing)
            {
                gameHandler.PlayProccessSound();
                isProcessing = true;
            }
            if (timeProcessLeft <= 0)
            {                
                energy = Mathf.Max(0, Mathf.Min((energy + particle.Value), maxEnergy));
                if (isSelected) gameHandler.DebugManager.LogText("Partícula procesada");
                if (isSelected) gameHandler.DebugManager.LogText("Valor energético = " + particle.Value);
                Debug.Log("Value = " + particle.Value);
                particle.Process();
                state = cellState.Waiting;
                cellInfo.UpdateEnergy(energy);
                cellInfo.AddTask(new Task(TaskObject.TaskType.process, "0"));
                timeProcessLeft = timeProcess;
                isProcessing = false;
            }          
        }
        else
        {
            Debug.Log("Particle not found at Cell.processParticle()");
        }
    }

    //Crea otra célula a cambio de la mitad de su energía
    public void DivideCell()
    {
        timeDivisionLeft -= Time.deltaTime;
        if (timeDivisionLeft <= 0)
        {
            energy = energy / 2;
            gameHandler.AddCell(energy, position);
            if (isSelected) gameHandler.DebugManager.LogText("La célula se ha dividido");
            Debug.Log("Cell divided");
            state = cellState.Waiting;
            cellInfo.UpdateEnergy(energy);
            cellInfo.AddTask(new Task(TaskObject.TaskType.divide, "0"));
            //animator.SetInteger("CellState", 0);
            timeDivisionLeft = timeDivision;
        }
    }

    //Destruye la partícula recibida como parámetro
    public void DestroyParticle(Particle particle)
    {
        if (particles.Contains(particle))
        {
            timeProcessLeft -= Time.deltaTime;
            if (timeProcessLeft <= 0)
            {
                energy -= 2;
                Debug.Log("Particle destroyed");
                int particleID = particles.IndexOf(particle);
                Destroy(particles[particleID].gameObject);
                particles.RemoveAt(particleID);
                size--;
                cellInfo.UpdateEnergy(energy);
                cellInfo.UpdateSize(size);
                cellInfo.AddTask(new Task(TaskObject.TaskType.destroyParticle, "0"));
                state = cellState.Waiting;
            }
        }
        else
        {
            Debug.Log("Particle not found at Cell.DestroyParticle()");
        }
    }

    //Elimina las partículas dentro de la célula
    public void RemoveAll()
    {
        state = cellState.Waiting;
        taskSystem = null;
        for(int i = 0; i < particles.Count; i++)
        {
            Destroy(particles[i].gameObject);
        }
        particles.Clear();
        size = 0;
    }
    
    //Cambia el color de la célula
    public void setColor(string color)
    {
        switch (color)
        {
            case "Red":
                spriteRenderer.color = new Color(1, 0.5f, 0.5f);
                break;
            case "Green":
                spriteRenderer.color = new Color(0.5f, 1, 0.5f);
                break;
            case "Blue":
                spriteRenderer.color = new Color(0.5f, 0.5f, 1);
                break;
            case "Cyan":
                spriteRenderer.color = new Color(0.5f, 1, 1);
                break;
            case "Magenta":
                spriteRenderer.color = new Color(1, 0.5f, 1);
                break;
            case "Yellow":
                spriteRenderer.color = new Color(1, 1, 0.5f);
                break;
            case "Black":
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
                break; 
            case "White":
                spriteRenderer.color = new Color(1, 1, 1);
                break;
        }
        //Actualiza el color de las vesículas de las partículas en su interior
        foreach(Particle particle in particles)
        {
            particle.UpdateColor(spriteRenderer.color);
        }
    }

    //Cambia la célula seleccionada
    public void Select()
    {
        isSelected = !isSelected;
        energyBar.SetVisible(isSelected);
    }

    private void OnMouseDown()
    {
        Select();
    }

    public void Destroy()
    {
        gameHandler.RemoveCell(this);
    }
}
