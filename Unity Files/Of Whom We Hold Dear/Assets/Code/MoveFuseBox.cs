using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFuseBox : MonoBehaviour {

    public GameObject fuseBox;
    public GameObject resetBox;
    public Transform[] fuseboxPositionNodes;
    public Vector3 rotation;

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

        fuseBox.transform.rotation = Quaternion.Euler(fuseboxPositionNodes[randomTransformFuseBox].GetComponent<FuseBoxRotation>().Rotation());
        resetBox.transform.rotation = Quaternion.Euler(fuseboxPositionNodes[randomTransformResetBox].GetComponent<FuseBoxRotation>().Rotation());
    }
}
