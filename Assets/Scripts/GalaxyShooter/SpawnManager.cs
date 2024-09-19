using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] List<GameObject> _prefabs;
    private bool _running = true;
    private GameManager _game_mgr;
    private GameObject _player_container;
    private GameObject _enemy_container;
    private GameObject _pu_container;


    void Start()
    {
        _game_mgr = GameManager._instance;
        if ( _game_mgr == null ) 
            Debug.LogError("Game Manager is null");

        _enemy_container = new GameObject();
        _enemy_container.transform.parent = this.transform;
        _enemy_container.name = "Enemies";
        _pu_container = new GameObject();
        _pu_container.transform.parent = this.transform;
        _pu_container.name = "PowerUps";
        _player_container = new GameObject();
        _player_container.transform.parent = this.transform;
        _player_container.name = "Players";

        if ( _game_mgr.GetNumPlayers() == 1 ) {
            Vector3 spawn_pos = new Vector3(0,_game_mgr._screen_bounds.y/2,0);
            GameObject player = Instantiate(_prefabs[0],spawn_pos, Quaternion.identity, transform);
            player.transform.parent = _player_container.transform;
            player.name = "Player";
            // set spawn and boundaries
        }
        else {
            for ( int i = 0; i < _game_mgr.GetNumPlayers(); i++ ){
                GameObject player = Instantiate(_prefabs[0],new Vector3((_game_mgr._screen_bounds.x/2)*(i*-1),_game_mgr._screen_bounds.y/2,0), Quaternion.identity, transform);
                player.transform.parent = _player_container.transform;
                player.name = "Player";
                // Set spawn location & boundaries
            }
        }
        
        StartCoroutine(StartEnemySpawn());  
        StartCoroutine(StartPowerUpSpawn());  
        StartCoroutine(StartAsteroidSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Stop() {
        _running = false;
        StopAllCoroutines();

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
            Destroy(obj, 0.5f);
        }

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ShieldPU")) {
            Destroy(obj, 0.5f);
        }

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("TripleShot")) {
            Destroy(obj, 0.5f);
        }

            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Boost")) {
            Destroy(obj, 0.5f);
        }

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Asteroid")) {
            Destroy(obj, 0.5f);
        }
    }

    IEnumerator StartEnemySpawn() {
        while (_running == true) {
            Vector3 spawn_pos = new Vector3(Random.Range(_game_mgr._screen_bounds.x, _game_mgr._screen_bounds.x *-1),(_game_mgr._screen_bounds.y*-1)+3,0);
            GameObject enemy = Instantiate(_prefabs[1], spawn_pos, Quaternion.identity, transform);
            enemy.transform.parent = _enemy_container.transform;
            float waitTime = Random.Range(0.5f,3f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator StartAsteroidSpawn() {
        while (_running == true) {
            Vector3 spawn_pos = new Vector3(Random.Range(_game_mgr._screen_bounds.x, _game_mgr._screen_bounds.x *-1),(_game_mgr._screen_bounds.y*-1)+3,0);
            Instantiate(_prefabs[2], spawn_pos, Quaternion.identity, transform);
            float waitTime = Random.Range(4f,9f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator StartPowerUpSpawn() {
        while (_running == true) {
            Vector3 spawn_pos = new Vector3(Random.Range(_game_mgr._screen_bounds.x, _game_mgr._screen_bounds.x *-1),(_game_mgr._screen_bounds.y*-1)+3,0);
            GameObject pu = Instantiate(_prefabs[Random.Range(3,_prefabs.Count)], spawn_pos, Quaternion.identity, transform);
            pu.transform.parent = _pu_container.transform;
            float waitTime = Random.Range(10f,20f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
