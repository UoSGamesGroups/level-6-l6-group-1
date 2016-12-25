using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class CarerNavigation : MonoBehaviour {

    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public GameObject player;
    public Vector3 playerLocation;
    public bool stopChasing;
    public FuseBox fusebox;
    public Transform startPosition;
    public GameObject CarerLight;
    public Transform[] carerMovementNodes;
    public PlayerMovement playermovement;
    public int nodePosition;
    public bool foundPlayer;
    public Transform target;
    public Animator animation;
    public bool searching;

    // Use this for initialization
    void Start () {
        transform.position = startPosition.position;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public int getNewTransform()
    {
        int index = UnityEngine.Random.Range(0, carerMovementNodes.Length);
        return index;
    }
	
	// Update is called once per frame
	void Update () {

        navMeshAgent.updateRotation = true;

        if(fusebox.lastArray)
        {
            stopChasing = true;
            CarerLight.SetActive(true);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = 8;
        }
        else
        {
            stopChasing = false;
            CarerLight.SetActive(false);
            foundPlayer = false;
            animation.SetBool("Finding", false);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = 4;
        }
        if (playermovement.carerTrigger && !foundPlayer)
        {
            navMeshAgent.SetDestination(carerMovementNodes[nodePosition].position);
        }
        if (playermovement.carerTrigger && foundPlayer)
        {
            navMeshAgent.SetDestination(player.transform.position);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = 10;

        }

       if(stopChasing && !foundPlayer && playermovement.carerTrigger)
        {
            animation.SetBool("Finding", true);
            RaycastHit hit;
            Ray newRay = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(newRay, out hit, 500)) 
            {
                Debug.DrawLine(transform.position, target.position);

                Debug.Log(hit.collider.tag);

                if(hit.collider.tag == "Player")
                {
                    foundPlayer = true;
                    animation.SetBool("Finding", false);
                }
 
            }
        }

        if (foundPlayer && navMeshAgent.remainingDistance < 5f)
        {
            // do something
        }

        if (navMeshAgent.remainingDistance < 0.2f)
        {
            nodePosition = getNewTransform();
        }
        
	}
}
