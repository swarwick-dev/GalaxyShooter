using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 _position;
    private float _r_speed = 19f;
    private float _acceleration = 3f; 
    private Player _player;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _explode_sound;
    private Animator _anim;
    private GameManager _game_mgr;

    // Start is called before the first frame update
    void Start()
    {
        _game_mgr = GameManager._instance;
        if ( _game_mgr == null ) 
           Debug.LogError("Game Manager is null");

        _position = transform.position;   

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
        transform.Rotate(Vector3.forward * _r_speed * Time.deltaTime);
        Vector3 movement = Vector3.down * _acceleration * Time.deltaTime;
        transform.Translate(movement, Space.World );
        _position = transform.position;
        
        if ( _game_mgr.HitLowerBounds(this.gameObject) ){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if other is player
        if ( other.tag == "Player" ) {
            _player = other.transform.GetComponent<Player>();
            _player.DamagePlayer(100);            
            Destroyed();
        }

        if ( other.tag == "Shield" ) {
            _player = other.transform.parent.GetComponent<Player>();
            _player.DamageShield(75);
            Destroyed();
        }

        // if other laser 
        if ( other.tag == "Laser" ) {
            if ( other.GetComponent<Laser>()._owner == "Player"){
                _player =  GameObject.Find(other.GetComponent<Laser>()._owner).GetComponent<Player>();
                _player.AddPoints(30);
            }
            Destroy(other.gameObject);
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

