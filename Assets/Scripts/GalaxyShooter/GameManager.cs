using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private int _num_players = 1;

    // Screen bounds
    public Vector3 _screen_bounds;

    [SerializeField] private GameObject _spawn_mgr_prefab;
    [SerializeField] private GameObject _pause_menu;
    [SerializeField] private GameObject _name_panel;
    [SerializeField] private Text _name_text;
    private GameObject _spawn_mgr;
    private UIManager _ui_manager;
    [SerializeField] private AudioSource _sfx_audio;
    private HighScoreTable _scores;
    private long _score;
   
    void Awake() {
        // persist this object
         _instance = this;
        DontDestroyOnLoad(gameObject);
        _screen_bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 0.5f, Screen.height, Camera.main.transform.position.z));
        _ui_manager = this.GetComponent<UIManager>();
        if ( _ui_manager == null ) {
            Debug.LogError("Audio Source is null");
        }

        _scores = this.GetComponent<HighScoreTable>();
        if ( _scores == null ) {
            Debug.LogError("High Score is null");
        }

        //_sfx_audio = this.AddComponent<AudioSource>();
        if ( _sfx_audio == null ) {
            Debug.LogError("Audio Source is null");
        }
    }

    public UIManager GetUIManager() {
        return _ui_manager;
    }

    public void StartGame(){

    }

    public void EndGame() {
        if ( _spawn_mgr != null ) {
            Destroy(_spawn_mgr);
        }

        Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
        foreach(var trans in transforms) {
            if ( trans.tag == "InGameUI") 
                trans.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.P) == true) {
            // Pause game
            Time.timeScale = 0;
            _pause_menu.SetActive(true);

        }
    }

    public void Resume() {
        _pause_menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Quit() {
        EndGame();
        _pause_menu.SetActive(false);
        Time.timeScale = 1;

        _ui_manager._game_over.gameObject.SetActive(false);

        PanelManager pm = Component.FindFirstObjectByType<PanelManager>() as PanelManager;
        pm.OpenPanel(pm.initiallyOpen);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        Destroy(_spawn_mgr.gameObject);
        _ui_manager.ResetUI();
        _spawn_mgr = Instantiate(_spawn_mgr_prefab, transform);

    }

    public AudioSource GetSFXAudioSource() {
        return _sfx_audio;
    }

    public void PlaySFXSound(AudioClip sound) {
        _sfx_audio.clip = sound;
        _sfx_audio.Play();
    }

    public SpawnManager GetSpawnManager() {
        return _spawn_mgr.GetComponent<SpawnManager>();
    }


    public void GameOver(long score) {
        _score = score;
        // Shut everything down cleanly !
        // Retain score but loops objects to destroy
        
        // Disable GameCanvas and stop SpawnManager
        Destroy(_spawn_mgr.gameObject);
        Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
        foreach(var trans in transforms) {
            if ( trans.tag == "InGameUI") 
                trans.gameObject.SetActive(false);
        }

        // Check if the score is a highscore
        if ( _scores.IsNewHighScore(_score) == true) {
            // Popup to add name 
            _name_panel.SetActive(true);
        } else {
            Finished();
        }        
    }

    private void Finished() {
        _name_panel.SetActive(false);
        _pause_menu.SetActive(true);
        foreach( var b in _pause_menu.GetComponentsInChildren<UnityEngine.UI.Button>()) {
            if ( b.name == "ResumeButton")
                b.interactable = false;
        }
    }

    public void AddScore() {
        
        _scores.AddHighScoreEntry(_score,_name_text.text);
        _name_text.text = "";
        _name_panel.SetActive(false);
        Finished();
    }

    public bool InBounds(GameObject obj){
        Vector3 currPos = obj.transform.position;
        Vector3 newPos = currPos;
        newPos.x = Mathf.Clamp(newPos.x, _screen_bounds.x, _screen_bounds.x * -1);
        newPos.y = Mathf.Clamp(newPos.y, _screen_bounds.y, _screen_bounds.y * -1);
        obj.transform.position = newPos;
        return currPos != newPos;
    }

    public bool HitLowerBounds(GameObject obj){
        if ( _screen_bounds.y + -3f > obj.transform.position.y )
            return true;
        
        return false;
    }

    public bool HitUpperBounds(GameObject obj){
        if ( (_screen_bounds.y + -3f)*-1 > obj.transform.position.y )
            return true;
        
        return false;
    }

    public void LoadGameScreen(int players) {
        _screen_bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _num_players = players;

        _ui_manager.ResetUI();

        _spawn_mgr = Instantiate(_spawn_mgr_prefab, transform);
    }

    public int GetNumPlayers() {
        return _num_players;
    }
}
