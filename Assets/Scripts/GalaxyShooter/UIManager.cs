using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score_text;
    [SerializeField]
    private Text _health_text;
    [SerializeField]
    private Text _shield_text;
    [SerializeField]
    private List<Sprite> _life_sprites;
    [SerializeField]
    private Image _life_img;
    [SerializeField]
    private Text _game_over;
    [SerializeField]
    private Text _restart_text;
    private GameManager _game_mgr;

    // Start is called before the first frame update
    void Start()
    {
        _game_over.gameObject.SetActive(false);
        _score_text.text = "Score : " + 0;
        _health_text.text = "Health : 100%";
        _health_text.color = Color.green;
        _shield_text.text = "Shield : 0%";
        _shield_text.color = Color.red;
        _life_img.sprite = _life_sprites[3];
        _restart_text.gameObject.SetActive(false);

        _game_mgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        if ( _game_mgr == null ) 
            Debug.LogError("GameManager is null");
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScore(int score) {
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
        StartCoroutine(GameOverFlash());
        _restart_text.gameObject.SetActive(true);

        _game_mgr.GameOver();
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
