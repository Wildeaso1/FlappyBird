using System;
using System.Collections.Generic;
using Data;
using Framework.Audio;
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
        [SerializeField] private int poolSize = 10;
        [SerializeField] private float multiWallChance = 0.1f;

        private readonly (WallDirection first, WallDirection second)[] validCombinations =
        {
            (WallDirection.LEFT, WallDirection.UP),
            (WallDirection.LEFT, WallDirection.DOWN),
            (WallDirection.RIGHT, WallDirection.UP),
            (WallDirection.RIGHT, WallDirection.DOWN),
        };
        
        public AudioPlayer AudioPlayer { get; private set; }

        private GameData _gameData;
        private Dictionary<GameObject, Queue<GameObject>> _wallPools;
        
        private bool _firstSpawn;
        private bool _spawningMultiWall;
        private int _wallsLength;
        private float _lastSpawnTime;

        private void Awake()
        {
            AudioPlayer = GetComponent<AudioPlayer>();
            _wallsLength = walls.Length;
            _gameData = GetComponent<GameData>();
            InitializePools();
        }

        private void InitializePools()
        {
            _wallPools = new Dictionary<GameObject, Queue<GameObject>>();
            
            foreach (var wallObject in walls)
            {
                var pool = new Queue<GameObject>();
                for (int i = 0; i < poolSize; i++)
                {
                    var pooledWall = Instantiate(wallObject.wall);
                    pooledWall.SetActive(false);
                    pool.Enqueue(pooledWall);
                }
                _wallPools[wallObject.wall] = pool;
            }
        }

        private float GetCurrentMultiWallChance()
        {
            float baseChance = multiWallChance;
            float scoreBonus = _gameData.Score * 0.005f;
            return Mathf.Min(0.8f, baseChance * scoreBonus);
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
            
            bool spawnMultiWall = Random.value < GetCurrentMultiWallChance() && _gameData.Score >= 1;
            print($"{spawnMultiWall} Chance: {GetCurrentMultiWallChance()} Score: {_gameData.Score}"); 
            
            if (spawnMultiWall && activeWalls.Count < spawnCount)
                SpawnMultiWall();
            else
                SpawnSingleWall();
            
            _lastSpawnTime = Time.time;
        }

        private void SpawnSingleWall()
        {
            var r = Random.Range(0, walls.Length);
            SpawnWallAtPosition(walls[r]);
        }

        private void SpawnMultiWall()
        {
            _spawningMultiWall = true;
            var combination = validCombinations[Random.Range(0, validCombinations.Length)];
            print("Spawning MultiWall");
            
            WallsObject firstWall = null;
            WallsObject secondWall = null;
            foreach (var wall in walls)
            {
                if (wall.wallDirection == combination.first && firstWall == null)
                    firstWall = wall;
                if (wall.wallDirection == combination.second && secondWall == null)
                    secondWall = wall;
            }
            if (firstWall != null && secondWall != null)
            {
                SpawnWallAtPosition(firstWall, true);
                SpawnWallAtPosition(secondWall);
            }
            else
                SpawnSingleWall();
            
            _spawningMultiWall = false;
        }

        private void SpawnWallAtPosition(WallsObject wall, bool isFirstOfMulti = false)
        {
            var prefab = wall.wall;
            var spawnPosition = wall.spawnPosition;
            switch (wall.wallDirection)
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
                if (!_spawningMultiWall)
                    spawnPosition.z = lastWall.transform.position.z + Random.Range(5,20);
                else
                {
                    if (isFirstOfMulti)
                        spawnPosition.z = lastWall.transform.position.z + Random.Range(5, 20);

                    else
                        spawnPosition.z = lastWall.transform.position.z;
                }
            }
            else
                _firstSpawn = false;
                
            
            var spawnedWall = GetPooledWall(prefab);
            if (spawnedWall != null)
            {
                spawnedWall.transform.position = spawnPosition;
                spawnedWall.transform.rotation = prefab.transform.rotation;
                spawnedWall.SetActive(true);
                activeWalls.Add(spawnedWall);
            }
        }
        private GameObject GetPooledWall(GameObject prefab)
        {
            if (_wallPools.ContainsKey(prefab) && _wallPools[prefab].Count > 0)
            {
                return _wallPools[prefab].Dequeue();
            }
            
            return Instantiate(prefab);
        }

        public void RemoveWall(GameObject wall)
        {
            if (wall)
            {
                activeWalls.Remove(wall);
                ReturnWallToPool(wall);
            }
        }

        private void ReturnWallToPool(GameObject wall)
        {
            wall.SetActive(false);
            
            foreach (var wallObject in walls)
            {
                if (wall.name.Contains(wallObject.wall.name))
                {
                    _wallPools[wallObject.wall].Enqueue(wall);
                    return;
                }
            }
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
                    wall.speed = 15f;
                    break;
                case 40:
                    wall.speed = 20f;
                    break;
                case 50:
                    wall.speed = 25f;
                    break;
                case 75: 
                    wall.speed = 30f;
                    break;
                case 100:
                    wall.speed = 35f;
                    break;
                default:
                    break;
            }
        }
    }
}
