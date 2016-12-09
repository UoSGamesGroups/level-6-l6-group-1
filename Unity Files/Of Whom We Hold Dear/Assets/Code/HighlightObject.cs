using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    public Color startcolor;
    public Color hovercolor;
    public bool mouseOver = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mouseOver = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mouseOver = false;
        }
    }

    void OnMouseEnter()
    {
        if (mouseOver)
        {
           GetComponent<Renderer>().material.SetColor("_Color", hovercolor);
        }
           

        }
        void OnMouseExit()
        {
        if (!mouseOver)
            {
            GetComponent<Renderer>().material.SetColor("_Color", startcolor);
            }
        }
 }