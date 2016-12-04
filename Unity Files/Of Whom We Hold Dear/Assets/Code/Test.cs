using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

    int index;

    void NextScene()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        index ++;
        if(index == 6)
        {
            index = 0;
        }
        SceneManager.LoadScene(index);
    }
}
