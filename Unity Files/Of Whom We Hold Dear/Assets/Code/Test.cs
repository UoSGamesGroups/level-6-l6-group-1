using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

    int index;
    public GameObject carer;

    public void NextScene()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        index ++;
        if(index == 6)
        {
            index = 0;
        }
        SceneManager.LoadScene(index);
    }
    public void ShowCarer()
    {
        if(carer.activeSelf)
        {
            carer.SetActive(false);
        }else
        {
            carer.SetActive(true);
        }
    }
}
