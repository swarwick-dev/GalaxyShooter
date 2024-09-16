using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float _acceleration = 4f;
    private Vector3 _position;
    private float _y_max = 6.5f;
    private float _y_min = -5f;
    private float _x_min = -8f;
    private float _x_max = 8f;
    private Player _player;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _explode_sound;
    private Animator _anim;
    [SerializeField] GameObject _laser_prefab;
    private float _can_fire = 0f;
    private float _fire_rate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(_x_min, _x_max),_y_max,0);
        _position = transform.position;   
        _player = GameObject.Find("Player").GetComponent<Player>();
        if ( _player == null ) 
            Debug.LogError("Player is null");

        _anim = this.GetComponent<Animator>();
        if ( _anim == null )
            Debug.LogError("Animator is null");
        _audio_source = GetComponent<AudioSource>();
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

    ///private void OnTriggerEnter(Collider other) {
    private void OnTriggerEnter2D(Collider2D other) {
        // if other is player
        if ( other.tag == "Player" ) {
            _player.DamagePlayer(50);            
            Destroyed();
        }

        if ( other.tag == "Shield" ) {
            _player.DamageShield(50);
            Destroyed();
        }

        // if other laser 
        if ( other.tag == "Laser" ) {
            Destroy(other.gameObject);
            _player.AddPoints(10);
            Destroyed();
        }
    }

    void moveEnemy() {
        Vector3 direction = new Vector3(0,-1,0);
        Vector3 movement = direction * _acceleration * Time.deltaTime;
        transform.Translate(movement );
        _position = transform.position;

        if ( _position.y <= _y_min ){
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
        _fire_rate = Random.Range(3f,6f);
        _can_fire = Time.time + _fire_rate;
        GameObject laser = Instantiate(_laser_prefab, transform.position, Quaternion.identity);
        //laser.transform.parent = this.transform;
    }
}
