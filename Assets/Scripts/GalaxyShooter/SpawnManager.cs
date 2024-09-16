using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _running = true;
    [SerializeField]
    GameObject _enemy_prefab;
    [SerializeField]
    GameObject _enemy_container;
    [SerializeField]
    GameObject _player_prefab;
    [SerializeField]
    List<GameObject> _pu_prefab;
    [SerializeField]
    GameObject _pu_container;
    [SerializeField]
    GameObject _asteroid_prefab;
    private int _enemy_count = 0;

    void Start()
    {
        _enemy_count = 0;
        //Instantiate(_player_prefab);

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
        foreach(var obj in GameObject.FindWithTag("Enemy").GetComponents<Enemy>())
            Destroy(obj.gameObject);
    }

    IEnumerator StartEnemySpawn() {
        while (_running == true) {
            GameObject enemy = Instantiate(_enemy_prefab);
            enemy.transform.parent = _enemy_container.transform;
            float waitTime = Random.Range(0.5f,3f);
            _enemy_count++;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator StartAsteroidSpawn() {
        while (_running == true) {
            Instantiate(_asteroid_prefab);
            float waitTime = Random.Range(4f,9f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator StartPowerUpSpawn() {
        while (_running == true) {
            GameObject pu = Instantiate(_pu_prefab[Random.Range(0,_pu_prefab.Count)]);
            pu.transform.parent = _pu_container.transform;
            float waitTime = Random.Range(3,8);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
