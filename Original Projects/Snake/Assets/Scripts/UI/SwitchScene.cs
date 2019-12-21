using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A simple helper script that provides a method for changing scenes. This can be utilized by
// buttons that change the scene.
public class SwitchScene : MonoBehaviour
{

    public string sceneName;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}