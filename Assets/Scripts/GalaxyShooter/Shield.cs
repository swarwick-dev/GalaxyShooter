using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool _taken_hit;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _taken_hit = false;
        _player = GameObject.Find("Player").GetComponent<Player>();
        if ( _player == null ) 
            Debug.LogError("Player is null");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // if other is player
        if ( other.tag == "EnemyLaser" ) {

            Destroy(other.gameObject);

            if ( _taken_hit == false ) {
                _taken_hit = true;
                StartCoroutine(BeenHit());
                _player.DamageShield(50);
            }            
        }
    }

        IEnumerator BeenHit() {
        yield return new WaitForSeconds(0.2f);
        _taken_hit = false;
    }
}
