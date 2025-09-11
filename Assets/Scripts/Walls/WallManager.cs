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
        [SerializeField] private int spawnCount;
        
        private bool _firstSpawn;

        private void Start() => SpawnWall();

        private void Update()
        {
            if (activeWalls.Count >= spawnCount)
                return;
            
            SpawnWall();
        }

        private void SpawnWall()
        {
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].wall;
            var spawnPosition = walls[r].spawnPosition;

            if (!_firstSpawn && activeWalls.Count > 0)
            {
                var lastWall = activeWalls[activeWalls.Count - 1];
                spawnPosition.z = lastWall.transform.position.z + Random.Range(5,20);
            }
            else
                _firstSpawn = false;
            
            var spawnedWall = Instantiate(prefab,spawnPosition, prefab.transform.rotation);
            
            activeWalls.Add(spawnedWall);
        }

        public void RemoveWall(GameObject wall)
        {
            activeWalls.Remove(wall);
        }
    }
}
