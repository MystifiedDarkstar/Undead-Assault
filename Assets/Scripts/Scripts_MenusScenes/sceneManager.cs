using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{

    public void openLevel(int sceneIndex) {

        SceneManager.LoadScene(sceneIndex);

    }

    public void quitGame() {

        Application.Quit();    

    }
}
