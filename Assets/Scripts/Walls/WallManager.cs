using System;
using System.Collections.Generic;
using Framework.Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Walls
{
    public class WallManager : MonoBehaviour
    {
        [SerializeField] private WallsObject[] walls;
        [SerializeField] private List<GameObject> activeWalls;

        private void Start() => SpawnWall();

        private void Update()
        {
            if (activeWalls.Count >= 1)
                return;
            
            SpawnWall();
        }

        private void SpawnWall()
        {
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].wall;
            var spawnPosition = walls[r].spawnPosition;
            print($"{prefab.name} spawn position: {spawnPosition}"); ;
            
            Instantiate(prefab,spawnPosition, prefab.transform.rotation);
            activeWalls.Add(prefab);
        }
    }
}
