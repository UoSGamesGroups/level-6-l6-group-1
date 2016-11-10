using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Memory1()
    {

        Debug.Log("Memory1");
        SceneManager.LoadScene("Memory1");
    }
    void Memory2()
    {

        Debug.Log("Memory2");
        SceneManager.LoadScene("Memory2");
    }
    void Memory3()
    {

        Debug.Log("Memory3");
        SceneManager.LoadScene("Memory3");
    }
    void Memory4()
    {

        Debug.Log("Memory4");
        SceneManager.LoadScene("Memory4");
    }
    void Memory5()
    {
        SceneManager.LoadScene("Memory5");
    }
    void prologue_epilogue()
    {
        Debug.Log("prologue_epilogue");
        SceneManager.LoadScene("prologue_epilogue");
    }
}
