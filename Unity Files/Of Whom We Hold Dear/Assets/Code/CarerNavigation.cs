using UnityEngine;
using System.Collections;

public class CarerNavigation : MonoBehaviour {

    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public Vector3 playerLocation;
    public bool stopChasing;
    public FuseBox fusebox;
    public Transform returnPosition;
    public Transform startPosition;

    // Use this for initialization
    void Start () {
        transform.position = startPosition.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
    }
	
	// Update is called once per frame
	void Update () {

        if(!fusebox.EngagedGreenLight.activeSelf)
        {
            stopChasing = true;
        }
        else
        {
            stopChasing = false;
        }

        if (stopChasing)
        {
            playerLocation = player.transform.position;
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(returnPosition.position);
        }
        


	}
}
