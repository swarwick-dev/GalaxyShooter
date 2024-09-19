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
        _player = this.transform.parent.GetComponent<Player>();
        if ( _player == null ) 
            Debug.LogError("Player is null");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable() {
        this.enabled = true;
        _taken_hit = false;
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.enabled = false;
        _taken_hit = false;
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // if other is player
        if ( other.tag == "Laser" && other.GetComponent<Laser>()._owner == "Enemy") {
            Destroy(other.gameObject);
            if ( this.enabled == true ){
                if ( _taken_hit == false ) {
                    _taken_hit = true;
                    _player.DamageShield(50);
                    StartCoroutine(BeenHit());
                    
                }
            }
        }
    }

        IEnumerator BeenHit() {
        yield return new WaitForSeconds(0.2f);
        _taken_hit = false;
    }
}
