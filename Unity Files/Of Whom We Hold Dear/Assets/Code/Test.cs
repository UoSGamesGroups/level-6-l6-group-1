﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

    int index = 2;

    void Memory1()
    {
        SceneManager.LoadScene(1);
    }
    void NextScene()
    {
        SceneManager.LoadScene(index);
        index++;
    }
}