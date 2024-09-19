using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _acceleration = 3f;
    private AudioSource _audio_source;
    private GameManager _game_mgr;

    // Start is called before the first frame update
    void Start()
    {
        _game_mgr = GameManager._instance;
        if ( _game_mgr == null ) 
            Debug.LogError("Game Manager is null");
        
        _audio_source = _game_mgr.GetSFXAudioSource();
        if ( _audio_source == null )
            Debug.LogError("Audio Source is null");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.down * _acceleration * Time.deltaTime;
        transform.Translate(movement);
        if ( _game_mgr.HitLowerBounds(this.gameObject) ){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if other is player
        if ( other.tag == "Player" ) {
            Player player = other.transform.GetComponent<Player>();
            if ( player != null ) {
                _audio_source = this.gameObject.GetComponent<AudioSource>();
                _audio_source.Play();
                
                this.gameObject.SetActive(false);
                
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
