using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<string> sentences;
    private int sentenceCount;
    public Text headerText;
    public Text dialogueText;
    public Animator animator;
    
    private Dialogue dialogue;
    private AudioManager audioManager;

    public List<string> Sentences { get => sentences; set => sentences = value; }

    void Awake()
    {
        Sentences = new List<string>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }


    public void StartDialogue(Dialogue _dialogue)
    {
        animator.SetBool("isOpen", true);
        Sentences.Clear();
        sentenceCount = 0;
        dialogue = _dialogue;
        headerText.text = dialogue.header;

        foreach(string sentence in dialogue.sentences)
        {
            Sentences.Add(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        ButtonPress();
        if (sentenceCount >= Sentences.Count)
        {           
            EndDialogue();
            return;
        }
        string sentence = Sentences[sentenceCount];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        sentenceCount++;
    }

    public void DisplayPreviousSentence()
    {
        ButtonPress();
        if (sentenceCount <= 1)
        {
            return;
        }
        sentenceCount -= 2;
        string sentence = Sentences[sentenceCount];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        sentenceCount++;
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        TypeSound();
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        StopTypeSound();
    }

    void EndDialogue()
    {
        StopTypeSound();
        animator.SetBool("isOpen", false);
    }

    public void DialogueButton()
    {
        if (animator.GetBool("isOpen"))
        {
            EndDialogue();
        } else
        {
            StartDialogue(dialogue);
        }
    }

    public void ButtonPress()
    {
        if (audioManager != null)
        {
            audioManager.Play("ButtonPress2");
        }
    }

    public void TypeSound()
    {
        if(audioManager != null)
        {
            audioManager.Play("TextSound");
        }
    }

    public void StopTypeSound()
    {
        if (audioManager != null)
        {
            audioManager.Stop("TextSound");
        }
    }
}
