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
        [SerializeField] private float spawnDelay = 0.1f;
        private GameData _gameData;
        
        private bool _firstSpawn;
        private int _wallsLength;
        private float _lastSpawnTime;

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
            if (Time.time - _lastSpawnTime >= spawnDelay)
            {
                SpawnWall();
            }
        }

        private void SpawnWall()
        {
            if (_wallsLength == 0) return;
            
            var r = Random.Range(0, walls.Length);
            
            var prefab = walls[r].wall;
            var spawnPosition = walls[r].spawnPosition;
            print($"Spawn Position: {spawnPosition}");
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
            if (wall)
                activeWalls.Remove(wall);
        }
        public void IncreaseWallsSpeed()
        {
            foreach (var wall in walls)
            {
                IncreaseSpeed(wall);
            }
        }
        private void IncreaseSpeed(WallsObject wall)
        {
            switch (_gameData.Score)
            {
                case 0:
                    wall.speed = 5f;
                    break;
                case 10:
                    wall.speed = 8f;
                    break;
                case 20:
                    wall.speed = 10f;
                    break;
                case 30:
                    wall.speed = 12f;
                    break;
                case 50:
                    wall.speed = 15f;
                    break;
                case 100:
                    wall.speed = 20f;
                    break;
                default:
                    break;
            }
        }
    }
}
