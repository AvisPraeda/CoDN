using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase de las barras de información
public class InfoBar : MonoBehaviour
{
    private Transform bar;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        bar = transform.Find("Bar");
    }

    //Set the infoBar Size
    public void SetSize(float sizeNormalized)
    {
        bar.GetComponent<Transform>().localScale = new Vector3(sizeNormalized, 1f, 1f);
    }

    //Set the infoBar Color
    public void SetColor(Color color)
    {
        Debug.Log(color);
        bar.GetComponent<Image>().color = color;
    }

    //Show or hide the infoBar
    public void SetVisible(bool b)
    {
        animator.SetBool("isActive", b);
    }
}
