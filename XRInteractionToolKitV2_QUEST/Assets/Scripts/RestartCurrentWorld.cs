using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartCurrentWorld : MonoBehaviour
{
    public void btnRestartWorld(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
