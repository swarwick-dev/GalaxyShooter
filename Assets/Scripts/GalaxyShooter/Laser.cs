using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private int _direction;
    private Vector3 _position;
    private float _y_max = 8.0f;
    [SerializeField] private bool isEnemyLaser;
    private float _y_min = -5f;

    // Start is called before the first frame update
    void Start()
    {
        _position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveLaser();
    }

    void moveLaser() {

        transform.Translate(((isEnemyLaser == false) ? Vector3.up : Vector3.down) * _speed * Time.deltaTime)  ;
        _position = transform.position;

        if ( (isEnemyLaser == false && _position.y >= _y_max) ||
            (isEnemyLaser == true && _position.y <= _y_min) )
        {
            Destroy(this.gameObject);
        }
    }
}
