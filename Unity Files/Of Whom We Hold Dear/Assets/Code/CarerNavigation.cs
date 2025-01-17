using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class CarerNavigation : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    public Vector3 playerLocation;
    public bool stopChasing;
    public FuseBox fusebox;
   // public Transform startPosition;
    public GameObject CarerLight;
    public Transform[] carerMovementNodes;
    public PlayerMovement playermovement;
    public int nodePosition;
    public bool foundPlayer;
    public Transform target;
    public Animator animation;
    public bool searching;
    public int chaseSpeed;
    public int searchingSpeed;
    public int normalSpeed;
    public bool lookedAt;
    public GameObject closestNode;
    public float helpCarerFindTimer;
    public int posInArray;
    public float loseSightOfPlayer;
    public float caughtTimer;

    public Transform TopCapsule;
    public Transform BottomCapsule;
    public GameObject characterMesh;
    public GameObject carerFace;
    public bool playSoundClip;

    // Use this for initialization
    void Start()
    {
       // transform.position = startPosition.position;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    public int getNewTransform()
    {
        int index = UnityEngine.Random.Range(0, carerMovementNodes.Length);
        return index;
    }

    public GameObject DistanceBetweenPlayerNode()
    {
        float lastDistance = 500;
        int index = 0;

        foreach (var check in carerMovementNodes)
        {
            float dist = Vector3.Distance(player.transform.position, check.transform.position);

            if(dist < lastDistance && check.gameObject.transform.position.y >= player.transform.position.y - 5)
            {
                closestNode = check.gameObject;
                lastDistance = dist;
                posInArray = index;
            }
            index++;
        }
        lastDistance = 500;
        return closestNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= 0.2)
            {
                nodePosition = getNewTransform();
            }
        }

        if (stopChasing && !foundPlayer)
        {
            helpCarerFindTimer -= Time.deltaTime;
        }

        if(helpCarerFindTimer <= 0)
        {
            helpCarerFindTimer = 30;
            DistanceBetweenPlayerNode();
            nodePosition = posInArray;
        }

        if (!foundPlayer)
        {
            navMeshAgent.SetDestination(carerMovementNodes[nodePosition].position);
        }

        navMeshAgent.updateRotation = true;

        lookedAt = playermovement.isPlayerLookingAtCarer();

        if(!lookedAt && !stopChasing)
        {
            characterMesh.SetActive(false);
        }
        
        if (fusebox.lastArray)
        {
            stopChasing = true;
            CarerLight.SetActive(true);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = searchingSpeed;
            characterMesh.SetActive(true);
        }
        else
        {
            stopChasing = false;
            CarerLight.SetActive(false);
            foundPlayer = false;
            animation.SetBool("Finding", false);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = normalSpeed;

            if(lookedAt)
            {
                navMeshAgent.GetComponent<NavMeshAgent>().speed = 0;
                var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            }
        }
        if (foundPlayer)
        {
            navMeshAgent.SetDestination(player.transform.position);
            navMeshAgent.GetComponent<NavMeshAgent>().speed = chaseSpeed;
        }

        if (foundPlayer && navMeshAgent.remainingDistance <= 6.5f)
        {
            if(playermovement.puzzlecam3.enabled)
            {
                playermovement.PuzzleCameraSwap();
            }

            var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

            navMeshAgent.velocity = new Vector3(0, 0, 0);
            navMeshAgent.GetComponent<NavMeshAgent>().Stop();

            if (!playSoundClip)
            {
                Debug.Log("PLAY SOUND");
                //Playsound
                playSoundClip = true;
            }
             caughtTimer += Time.deltaTime;
             playermovement.TurnToFaceCarer();

                if (caughtTimer >=2)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
         }  
    }

    IEnumerable WaitForSeconds(int time)
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(time);
        Debug.Log(Time.time);
    }

    void FixedUpdate()
    {
        if (stopChasing)
        {
            if(!foundPlayer)
            {
                animation.SetBool("Finding", true);
            }

            RaycastHit hit;
            Vector3 point1 = TopCapsule.position;
            Vector3 point2 = BottomCapsule.position;
            
            if (Physics.CapsuleCast(point1,point2, 3f, transform.forward,out hit, 200))
            {
                //Debug.DrawLine(transform.position, hit.transform.position);
               // Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Player")
                {
                    foundPlayer = true;
                    animation.SetBool("Finding", false);
                  //  Debug.Log(hit.collider.tag);
                }
                if(foundPlayer && hit.collider.tag != "Player")
                {
                    loseSightOfPlayer -= Time.deltaTime;
                }
            }
        }
        if(loseSightOfPlayer <= 0)
        {
            foundPlayer = false;
            loseSightOfPlayer = 10;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foundPlayer = true;
            animation.SetBool("Finding", false);
        }
    }
}
