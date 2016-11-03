using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;

    public Camera cam1;
    public Camera cam2;

    public Animator Falling;

    void Start()
    {
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
