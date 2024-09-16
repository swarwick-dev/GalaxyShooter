using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //private float _acceleration = 3f;
    private Vector3 _position;
    private float _y_max = 6.5f;
    private float _y_min = -5f;
    private float _x_min = -8f;
    private float _x_max = 8f;
    private float _acceleration = 3f;
    private AudioSource _audio_source;
    [SerializeField] private AudioClip _pu_sound;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(_x_min, _x_max),_y_max,0);
        _position = transform.position; 

        _audio_source = GetComponent<AudioSource>();
        if ( _audio_source == null )
            Debug.LogError("Audio Source is null");

        if ( _pu_sound == null ) {
            Debug.Log("Missing sound : " + this.tag);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            Player player = other.transform.GetComponent<Player>();
            if ( player != null ) {
                if ( _audio_source == null )
                    Debug.LogError("Audio source is null : " + this.tag);
                _audio_source.clip = _pu_sound;
                _audio_source.Play();
                switch (transform.tag) {
                    case "TripleShot":
                        player.TripleShotCollected();
                        break;
                    case "Boost":
                        player.BoostCollected();
                        break;
                    case "ShieldPU":
                        player.ShieldCollected();
                        break;
                    default:
                        break;
                }
                
                Destroy(this.gameObject, 0.7f);
            }
        }
    }
}
