﻿using UnityEngine;
using System.Collections;

public class CamMouseLook : MonoBehaviour
{

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameObject character;
    public PlayerMovement playermovement;

    void Start()
    {
        character = this.transform.parent.gameObject;
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }


    void Update()
    {

        if (!playermovement.inAnimation)
        {
            var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
        }
        else
        {
            character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
        }

    }
}
