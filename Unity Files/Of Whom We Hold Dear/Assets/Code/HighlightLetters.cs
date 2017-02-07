using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightLetters : MonoBehaviour
{

    private Color StartColor;

    void OnMouseEnter()
    {
        StartColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.cyan;
    }
    
    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = StartColor;
    } 
 }
