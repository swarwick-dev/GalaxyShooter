using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 18f;
    private Vector3 _position;
    [SerializeField] private bool isEnemyLaser;
    private GameManager _game_mgr;
    public string _owner {set;get;}

    // Start is called before the first frame update
    void Awake()
    {
        _game_mgr = GameManager._instance;
        if ( _game_mgr == null ) 
           Debug.LogError("Game Manager is null");
           
        _position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         transform.Translate(((_owner == "Player") ? Vector3.up : Vector3.down) * _speed * Time.deltaTime);
        _position = transform.position;

        //if ( _game_mgr.HitLowerBounds(this.gameObject) ||
        //    _game_mgr.HitUpperBounds(this.gameObject) )
        if ( (_owner != "Player" && _position.y < _game_mgr._screen_bounds.y + -3) || 
            (_owner == "Player" && _position.y > (_game_mgr._screen_bounds.y*-1) + 3))
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Laser from " + transform.parent.tag + " hit " + other.tag);
    }
}
