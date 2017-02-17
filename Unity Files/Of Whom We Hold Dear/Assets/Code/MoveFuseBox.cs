using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFuseBox : MonoBehaviour {

    public GameObject fuseBox;
    public GameObject resetBox;
    public Transform fuseBoxEndLine;
    //public Transform resetBoxEndLine;
    public Transform[] fuseboxPositionNodes;
    public bool rotationComplete;
    public float roationY;
    public float fuseBoxYPos;
    public float resetBoxYPos;
    public FuseBoxRotation fuseBoxRotation;

    void Start()
    {
        MoveFuseBoxSystem();
    }

    public void MoveFuseBoxSystem()

    {
        int randomTransformFuseBox = Random.Range(0, fuseboxPositionNodes.Length);
        int randomTransformResetBox = Random.Range(0, fuseboxPositionNodes.Length);

        if (randomTransformResetBox == randomTransformFuseBox)
        {
            while (randomTransformResetBox == randomTransformFuseBox)
            {
                randomTransformResetBox = Random.Range(0, fuseboxPositionNodes.Length);
            }
        }

        fuseBox.transform.position = fuseboxPositionNodes[randomTransformFuseBox].transform.position;
        resetBox.transform.position = fuseboxPositionNodes[randomTransformResetBox].transform.position;

        fuseBoxYPos = RotateObject(fuseBox, randomTransformFuseBox);
        //resetBoxYPos = RotateObject(resetBox);

    }

    public float RotateObject(GameObject fuseBoxSystem, int posInArray)
    { 
        //Look at the enum get the direction and spin the object round till you get the right enum 
       

        return roationY;
    }
}
