using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _score_text;
    [SerializeField] private Text _health_text;
    [SerializeField] private Text _shield_text;
    [SerializeField] private List<Sprite> _life_sprites;
    [SerializeField] private Image _life_img;
    [SerializeField] public Text _game_over;
    private GameManager _game_mgr;
    [SerializeField] private GameObject _pause_menu;

    private long _current_score = 0;

    // Start is called before the first frame update
    public void Start()
    {
        _game_mgr = GameManager._instance;
        if ( _game_mgr == null ) 
            Debug.LogError("GameManager is null");   
    }

    public void ResetUI() {
        
        Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
            foreach(var trans in transforms) {
            if ( trans.tag == "InGameUI") 
                trans.gameObject.SetActive(true);
        }
        _game_over.gameObject.SetActive(false);
        _pause_menu.SetActive(false);

        foreach( var b in _pause_menu.GetComponentsInChildren<UnityEngine.UI.Button>()) {
            if ( b.name == "ResumeButton")
                b.interactable = true;
        }

        _score_text.text = "Score : " + 0;
        _health_text.text = "Health : 100%";
        _health_text.color = Color.green;
        _shield_text.text = "Shield : 0%";
        _shield_text.color = Color.red;
        _life_img.sprite = _life_sprites[3];

    }

    // Update is called once per frame

    public void UpdateScore(int score) {
        _current_score = score;
        _score_text.text = "Score : " + score;
    }

    public void UpdateLives(int lives) {
       _life_img.sprite = _life_sprites[lives];
    }

    public void UpdateShield(float shield) {
        _shield_text.text = "Shield : " + shield + "%";
        switch ( shield ) {
            case < 25: 
                _shield_text.color = Color.red;
                break;
            case < 50: _shield_text.color = Color.yellow;
                break;
            case < 75:_shield_text.color = Color.magenta;
                break;
            case > 75:
            default : _shield_text.color = Color.green;
                break;
        }
    }

   public void UpdateHealth(float health) {
        _health_text.text = "Health : " + health + "%";
        switch ( health ) {
            case < 25: 
                _health_text.color = Color.red;
                break;
            case < 50: _health_text.color = Color.yellow;
                break;
            case < 75:_health_text.color = Color.magenta;
                break;
            case > 75:
            default : _health_text.color = Color.green;
                break;
        }
    }

    public void GameOver() {
        _game_over.gameObject.SetActive(true);
        StartCoroutine(GameOverFlash());

        _game_mgr.GameOver(_current_score);
    }

    IEnumerator GameOverFlash()
    {
        _game_over.gameObject.SetActive(true);
        while(true)
        {
            _game_over.text = "GAME OVER !!!";
            yield return new WaitForSeconds(0.35f);
            _game_over.text = "";
            yield return new WaitForSeconds(0.35f);
        }
    }
}
