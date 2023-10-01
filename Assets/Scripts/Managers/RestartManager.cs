using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public static void Restart()
    {
        FindObjectOfType<RestartManager>().HandleRestart();
    }

    private void HandleRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
