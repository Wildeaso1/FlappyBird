using System;
using Framework.Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Walls
{
    public class WallManager : MonoBehaviour
    {
        [SerializeField] private WallsObject[] walls;

        private void Start()
        {
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].wall;
            var spawnPosition = walls[r].spawnPosition;
            print($"{prefab.name} spawn position: {spawnPosition}");
            
            Instantiate(prefab,spawnPosition , Quaternion.identity);
        }
    }
}
