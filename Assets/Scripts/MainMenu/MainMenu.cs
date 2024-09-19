using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load1PGame() {
        SceneManager.LoadScene(1);
    }

    public void Load2PGame() {
        SceneManager.LoadScene(2);
    }

    public void QuitGame () {
        Application.Quit();
    }
}
