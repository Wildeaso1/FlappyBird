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

            for (int i = 0; i < walls.Length; i++)
            {
                spawnPosition.z += Random.Range(5, 20) * i;
            }
            Instantiate(prefab,spawnPosition, prefab.transform.rotation);
            
            activeWalls.Add(prefab);
        }

        public void RemoveWall(GameObject wall)
        {
            activeWalls.Remove(wall);
        }
    }
}
