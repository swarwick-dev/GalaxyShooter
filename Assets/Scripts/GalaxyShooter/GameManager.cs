using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    public void GameOver() {
        _isGameOver = true;
    }

    public void Update() {
        if ( Input.GetKeyDown(KeyCode.R) == true && _isGameOver == true )
        {
            SceneManager.LoadScene(1);
        }
    }
}
