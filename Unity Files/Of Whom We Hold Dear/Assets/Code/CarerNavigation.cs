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
    public GameObject CarerLight;

    // Use this for initialization
    void Start () {
        transform.position = startPosition.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
    }
	
	// Update is called once per frame
	void Update () {

        if(fusebox.lastArray)
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
            CarerLight.SetActive(true);
        }
        else
        {
            navMeshAgent.SetDestination(returnPosition.position);
            CarerLight.SetActive(false);
        }
        


	}
}
