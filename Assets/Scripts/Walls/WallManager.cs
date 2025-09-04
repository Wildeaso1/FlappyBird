using System;
using Framework.Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Walls
{
    public class WallManager : MonoBehaviour
    {
        [SerializeField] private WallsObject[] walls;
        [SerializeField] private Vector3 spawnTransform;

        private void Start()
        {
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].Wall;
            
            Instantiate(prefab, spawnTransform, Quaternion.identity);
        }
    }
}
