using System;
using System.Collections.Generic;
using Data;
using Framework.Enums;
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
        private GameData _gameData;
        
        private bool _firstSpawn;
        private int _wallsLength;

        private void Awake()
        {
            _wallsLength = walls.Length;
            _gameData = GetComponent<GameData>();
        }

        private void Start() => SpawnWall();

        private void Update()
        {
            if (activeWalls.Count >= spawnCount)
                return;
            
            SpawnWall();
        }

        private void SpawnWall()
        {
            if (_wallsLength == 0) return;
            
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].wall;
            var spawnPosition = walls[r].spawnPosition;
            if (_gameData.Score >= 10) walls[r].speed = 8f;
            switch (walls[r].wallDirection)
            {
                case WallDirection.UP:
                    spawnPosition.y += 15f;
                    break;
                case WallDirection.DOWN:
                    spawnPosition.y -= 15f;
                    break;
                case WallDirection.LEFT:
                    spawnPosition.x -= 15f;
                    break;
                case WallDirection.RIGHT:
                    spawnPosition.x += 15f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!_firstSpawn && activeWalls.Count > 0)
            {
                var lastWall = activeWalls[^1];
                spawnPosition.z = lastWall.transform.position.z + Random.Range(5,20);
            }
            else
                _firstSpawn = false;
            var spawnedWall = Instantiate(prefab,spawnPosition, prefab.transform.rotation);
            
            activeWalls.Add(spawnedWall);
        }

        public void RemoveWall(GameObject wall)
        {
            if (wall != null)
                activeWalls.Remove(wall);
        }
    }
}
