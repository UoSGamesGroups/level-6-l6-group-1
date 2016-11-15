using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;
    public Transform[] spawnlocations; 

    public Camera cam1;
    public Camera cam2;

    public Animator animation;
    public Vector3 move;
    public bool lockcontrols = true;

    public GameController gamecontroller;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0, -35.0F, 0);
        cam1.enabled = true;
        cam2.enabled = false;

        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();

        Scene scene = SceneManager.GetActiveScene();

        if (gamecontroller.sceneName == "prologue_epilogue" || gamecontroller.sceneName == "ScaleExample")
        {
            move = spawnlocations[3].transform.position;
            transform.position = move;
        }

        else if (gamecontroller.sceneName == "Memory1" || gamecontroller.sceneName == "Memory2" || gamecontroller.sceneName == "Memory3")
        {
            int index = UnityEngine.Random.Range(0, 2);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }
        else 
        {
            int index = UnityEngine.Random.Range(0, spawnlocations.Length);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }

        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        lockcontrols = false;
        animation.SetBool("WakeUpProgression", true);
        GetComponent<Renderer>().enabled = false;

        StartCoroutine(waitforanimation(9,"WakeUpProgression"));

    }

    void ResetAnimations(string inprogressanimation)
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        animation.SetBool(inprogressanimation, false);
        GetComponent<Renderer>().enabled = true;
        lockcontrols = true;
    }

    void Update()
    {
        transform.Rotate(0, 90, 0);

        if (lockcontrols)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(straffe, 0, translation);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void ResetMovementControls()
    {
        lockcontrols = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fall")
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
            lockcontrols = false;
            animation.SetBool("Fall", true);
            GetComponent<Renderer>().enabled = false;
         }
    }

    IEnumerator waitforanimation(int waitTimer, string animationname)
    {
        yield return new WaitForSeconds(waitTimer);
        ResetAnimations(animationname);
    }
}
