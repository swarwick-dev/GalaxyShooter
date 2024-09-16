using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 _position;
    private Quaternion _rotation;
    private float _r_speed = 19f;
    private float _y_max = 6.5f;
    private float _y_min = -5f;
    private float _x_min = -8f;
    private float _x_max = 8f;
    private float _acceleration = 3f; 
    private Player _player;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _explode_sound;
    private Animator _anim;
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
        transform.Rotate(Vector3.forward * _r_speed * Time.deltaTime);
        Vector3 movement = Vector3.down * _acceleration * Time.deltaTime;
        transform.Translate(movement, Space.World );
        _position = transform.position;

        if ( _position.y <= _y_min ){
            Destroy(this.gameObject);
        }
    }

        private void OnTriggerEnter2D(Collider2D other) {
        // if other is player
        if ( other.tag == "Player" ) {
            _player.DamagePlayer(100);            
            Destroyed();
        }

        if ( other.tag == "Shield" ) {
            _player.DamageShield(75);
            Destroyed();
        }

        // if other laser 
        if ( other.tag == "Laser" ) {
            Destroy(other.gameObject);
            _player.AddPoints(30);
            Destroyed();
        }
    }
    void Destroyed() {
         _anim.SetTrigger("isDestroyed");
         Destroy(GetComponent<Collider2D>());
        //_acceleration = 0;
        //_r_speed = 0;
        _audio_source.clip = _explode_sound;
        _audio_source.Play();
        Destroy(this.gameObject, 2.633f);
    }
}

