using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;
    public Transform[] spawnlocations; 

    public Camera cam1;
    public Camera cam2;

    public Animator Falling;

    public GameController gamecontroller;
   // public Animator Wakingup; //Yet to make the animation 

    void Start()
    {

        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
        //Play the waking up animation first in the hallway, Spawn us in the hallway always if the scene is prolouge
        //If it is before memory 4 always spawn in the dining,living or main area. 4 & and 5 allow you to spawn in your bedroom as well. 



        //Select Where the player will spawn 
        Scene scene = SceneManager.GetActiveScene();

        if (gamecontroller.sceneName == "Memory1")
        {
            Debug.Log("Active scene is '" + scene.name + "'.");

        }
        



        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0, -35.0F, 0);
        cam1.enabled = true;
        cam2.enabled = false;

    }

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;

        transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fall")
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
            Falling.SetBool("Fall", true);
            GetComponent<Renderer>().enabled = false;
        }
    }
}
