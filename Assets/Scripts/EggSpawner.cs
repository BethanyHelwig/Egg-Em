using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EggSpawner : MonoBehaviour
{
    public ObjectPool<Egg> _pool;
    private GameBehavior gameBehavior;
    //[SerializeField] private Transform spawnPoint;

    private void Start()
    {
        _pool = new ObjectPool<Egg>(CreateEgg, OnTakeEggFromPool, ReturnEggToPool, OnDestroyEgg, true, 20, 40);
        gameBehavior = GetComponent<GameBehavior>();
    }

    private Egg CreateEgg()
    {
        Egg egg = Instantiate(gameBehavior.eggPrefab, gameBehavior.spawnPoint.position, gameBehavior.spawnPoint.rotation);

        egg.SetPool(_pool);

        return egg;
    }

    //what we want to do when we take an object from our object pool
    private void OnTakeEggFromPool(Egg egg)
    {
        // set the transform and rotation
        egg.transform.position = gameBehavior.spawnPoint.position;
        egg.transform.rotation = gameBehavior.spawnPoint.transform.rotation;

        //activate
        //egg.StateMachine.ChangeState(egg.ThrowState);
        egg.gameObject.SetActive(true);
        egg.StateMachine.ChangeState(egg.ThrowState);
    }

    private void ReturnEggToPool(Egg egg)
    {
        egg.gameObject.SetActive(false);
    }

    private void OnDestroyEgg(Egg egg)
    {
        Destroy(egg.gameObject);
    }
}
