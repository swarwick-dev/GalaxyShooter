using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    // Movement Boundaries
    static private float _x_min = -11.3f;
    static private float _x_max = 11.3f;
    static private float _y_min = -4f;
    static private float _y_max = 0f;
    
    // Player settings
    private float _acceleration = 5f;
    private Vector3 _position;
    static private int _lives = 3;
    private float _health {set; get;}
    private int _points {set; get;}

    // Power ups
    [SerializeField]
    private List<GameObject> _pu_prefab;
    private bool _hasTripleShot = false;
    static private float _ts_duration = 5.0f;
    private float _shield = 0.0f;
    static private float _shield_max = 200f;
    private bool _hasShield = false;
    private GameObject _shield_obj;
    static private float _boost_duration = 5.0f;
    static private float _boost_multiplier = 2f;
    static private float _default_speed = 5f;

    // Laser delay + cooldown
    [SerializeField]
    private GameObject _laser_prefab;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _explode_sound;
    [SerializeField] private AudioClip _laser_sound;
    private float _offset = 0.8f;
    private float _fire_delay = 0.01f;
    private float _next_fire = 0.0f;
    //private int _fired_count = 0;

    private SpawnManager _spawn;
    private UIManager _ui_manager;
    private float _max_health = 500f;
    List<GameObject> _l_damage;
    private Animator _anim;
    private bool _taken_hit;

    // Start is called before the first frame update
    void Start()
    {
        _l_damage = new List<GameObject>();
        transform.position = new Vector3(0,_y_min/2,0);
        _position = transform.position;
        _spawn = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if ( _spawn == null ) 
            Debug.LogError("SpawnManager is null");
        
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        foreach(var trans in transforms) {
            if ( trans.tag == "Shield") 
                _shield_obj = trans.gameObject;
            else if ( trans.tag == "PlayerDamage" )
                _l_damage.Add(trans.gameObject);
        }
        
        if ( _shield_obj == null ) 
            Debug.LogError("Shield is null");

        _ui_manager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if ( _ui_manager == null ) 
            Debug.LogError("UIManager is null");
        
        _shield_obj.SetActive(false);

        _anim = this.GetComponent<Animator>();
        if ( _anim == null )
            Debug.LogError("Animator is null");

        _audio_source = this.GetComponent<AudioSource>();
        if ( _audio_source == null )
            Debug.LogError("Audio Source is null");

        // Starting position 
        _points = 0;
        _health = 100f;
        _lives = 3;
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer(); 

  /*    #if UNITY_ANDROID
         xxx
        #elif UNITY_IOS
            xxx
        #else
            xxx
        #endif */      
  

        if ( Input.GetKeyDown(KeyCode.Space) == true && Time.time > _next_fire) {
            FireLaser();
        }
    }

    // Move the player based on key inputs and ensure it stays within bounds
    void movePlayer() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x,y,0);
        Vector3 movement = direction * _acceleration * Time.deltaTime;
        transform.Translate(movement );
        
        _position = transform.position;
        _position.y = Mathf.Clamp(_position.y, _y_min, _y_max);    

        if ( _position.x >= _x_max )
            _position.x = _x_min;
        else if ( _position.x <= _x_min )
            _position.x = _x_max;

        transform.position = _position;
    }

    void FireLaser() {
        Vector3 laser_pos = _position;
        
        if ( _hasTripleShot == false )
            laser_pos.y += _offset;

        Instantiate((_hasTripleShot == true) ? _pu_prefab[1] : _pu_prefab[0], laser_pos, Quaternion.identity);
        
        _next_fire = Time.time + _fire_delay;
        //_fired_count++;
        _audio_source.clip = _laser_sound;
        _audio_source.Play();

    }

    public void DamageShield(float damage) {

        if ( _hasShield == true ) {
            _shield -= damage;
            if ( _shield <= 0 ) {
                // destroy the shield
                _hasShield = false;
                damage = _shield * -1;
                _shield = 0;
                _shield_obj.SetActive(false);
                DamagePlayer(damage);
            }
            _ui_manager.UpdateShield(_shield);
        }
    }

    public void DamagePlayer(float damage) {
        if ( _hasShield == false ){
            _health = Mathf.Clamp(_health -= damage,0,_max_health);
            _ui_manager.UpdateHealth(_health);
            if ( _health == 0 )
            {
                if ( _l_damage.Count > 0 ) {
                    int i = UnityEngine.Random.Range(0,_l_damage.Count);
                    _l_damage[i].SetActive(true);
                    _l_damage.RemoveAt(i);
                }

                _lives = Mathf.Clamp(_lives-=1,0,3);              
                _ui_manager.UpdateLives(_lives);
                if ( _lives == 0 ) {
                    // Game over !
                    Transform[] transforms = GetComponentsInChildren<Transform>(true);
                    foreach(var trans in transforms) {
                        if ( trans.tag == "Shield" || trans.tag == "PlayerDamage" || trans.tag == "Thruster") 
                            trans.gameObject.SetActive(false);
                    }
                    Destroyed();
                } else {
                    Respawn();
                }
            }
        }
    }

    private void Respawn() {
        // Respawn
        _health = 100;
        _shield = 0;
        _acceleration = 5f;
        _hasTripleShot = false;
        _hasShield = false;
        _ui_manager.UpdateHealth(_health);
        _ui_manager.UpdateScore(_points);
        _ui_manager.UpdateShield(_shield);
        _taken_hit = false;
        _next_fire = 0f;

    }

    public int GetPoints() {
        return this._points;
    }

    public void AddPoints(int points) {
        _points += points;
        _ui_manager.UpdateScore(_points);
    }

    public float GetHealth() {
        return _health;
    }

    public float GetShield() {
        return _shield;
    }
    private IEnumerator TripleShotDisabled() {  
        yield return new WaitForSeconds(_ts_duration);
        _hasTripleShot = false;
    }
    public void TripleShotCollected() {
        _hasTripleShot = true;
        StartCoroutine(TripleShotDisabled());
    }

    private IEnumerator BoostDisabled() {  
        yield return new WaitForSeconds(_boost_duration);
        _acceleration = _default_speed;
    }
    public void BoostCollected() {
        _acceleration = _acceleration * _boost_multiplier;
        StartCoroutine(BoostDisabled());
    }

    public void ShieldCollected() {
        _shield = Mathf.Clamp(_shield + _shield_max, 0, _shield_max);
        if ( _hasShield == false ) {
            _hasShield = true;
            _shield_obj.SetActive(true);
        }
        _ui_manager.UpdateShield(_shield);
    }

    private void Destroyed() {
        _next_fire = Time.time + 30f;
        _spawn.Stop();
        _anim.SetTrigger("isDestroyed");
        _audio_source.clip = _explode_sound;
        _audio_source.Play();
        _acceleration = 0f;
        _ui_manager.GameOver();
        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // if other is player
        if ( other.tag == "EnemyLaser" ) {

            Destroy(other.gameObject);

            if ( _taken_hit == false ) {
                _taken_hit = true;
                StartCoroutine(BeenHit());
                this.DamagePlayer(50);
            }            
        }
    }

    IEnumerator BeenHit() {
        yield return new WaitForSeconds(0.2f);
        _taken_hit = false;
    }
}
