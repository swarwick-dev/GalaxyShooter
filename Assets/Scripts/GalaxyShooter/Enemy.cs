using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float _acceleration = 4f;
    private Player _player;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _explode_sound;
    private Animator _anim;
    [SerializeField] GameObject _laser_prefab;
    private float _can_fire = 0f;
    private float _fire_rate = 3f;
    private GameManager _game_mgr;
    private float _offset = -4f;

    // Start is called before the first frame update
    void Start()
    {
        _game_mgr = GameManager._instance;;
        if ( _game_mgr == null ) 
           Debug.LogError("Game Manager is null");

        _anim = this.GetComponent<Animator>();
        if ( _anim == null )
            Debug.LogError("Animator is null");
        _audio_source = _game_mgr.GetSFXAudioSource();
        if ( _audio_source == null )
            Debug.LogError("Audio Source is null");
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();

        if ( Time.time > _can_fire )
            FireLaser();
    }

    void OnDestroy()
    {
        Destroy(gameObject);
    }

    ///private void OnTriggerEnter(Collider other) {
    private void OnTriggerEnter2D(Collider2D other) {

        if ( other.tag == "Shield" ) {
            _player = other.transform.parent.GetComponent<Player>();
            _player.DamageShield(50);
            Destroyed();
        }

        // if other laser 
        if ( other.tag == "Laser" ) {
            if ( other.GetComponent<Laser>()._owner == "Player"){
                _player = GameObject.Find(other.GetComponent<Laser>()._owner).GetComponent<Player>();
                _player.AddPoints(10);
                Destroyed();
                Destroy(other.gameObject);
            } 
        }

        if ( other.tag == "Asteroid" ) {
            Destroyed();
        }
    }

    void moveEnemy() {
        Vector3 direction = new Vector3(0,-1,0);
        Vector3 movement = direction * _acceleration * Time.deltaTime;
        transform.Translate(movement );

        if ( _game_mgr.HitLowerBounds(this.gameObject) ){
            Destroy(this.gameObject);
        }
    }

    void Destroyed() {
         _anim.SetTrigger("OnEnemyDeath");
         Destroy(GetComponent<Collider2D>());
        //_acceleration = 0;
        if ( _audio_source.enabled == false )
            Debug.Break();
        _audio_source.clip = _explode_sound;
        _audio_source.Play();
        Destroy(this.gameObject, 2.633f);
    }

    void FireLaser() {
        _fire_rate = Random.Range(3f,5f);
        _can_fire = Time.time + _fire_rate;
        Vector3 laser_pos = transform.position;
        laser_pos.y += _offset;
        GameObject laser = Instantiate(_laser_prefab, transform.position, Quaternion.identity);
        laser.GetComponent<Laser>()._owner = "Enemy";
    }
}
