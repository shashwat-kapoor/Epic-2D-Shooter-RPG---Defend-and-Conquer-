using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void quitGame()
    {

        Application.Quit();
    }
}
