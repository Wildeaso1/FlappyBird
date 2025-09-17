using System;
using Data;
using Framework.Enums;
using Framework.Scriptable_Objects;
using Player;
using UnityEngine;

namespace Walls
{
    public class WallsMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private WallsObject  wall;
        
        private PlayerMovement _playerMovement;
        private GameObject _managerObject;
        private WallManager _wallManager;
        private GameData _gameData;
        private bool _isMovingIn = true;
        private bool _isMovingAway;
        private bool _isMovingTowardsPlayer;
        private Vector3 endPosition;
        private Vector3 startPosition;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            _playerMovement = player.GetComponent<PlayerMovement>();
            _managerObject = GameObject.FindWithTag("Manager");
            _gameData = _managerObject.GetComponent<GameData>();
            _wallManager = _managerObject.GetComponent<WallManager>();
        }

        private void OnEnable()
        {
            _isMovingIn = true;
            _isMovingAway = false;
            _isMovingTowardsPlayer = false;
            
            startPosition = new Vector3(wall.spawnPosition.x, wall.spawnPosition.y, transform.position.z);
        }

        private void Start()
        {
            startPosition.z = transform.position.z;
        }

        private void FixedUpdate() => MoveWall();

        private void MoveWall()
        {
            if (_gameData.IsGameOver)
                return;
            
            if (_isMovingIn)
            {
                MoveWallTowards(startPosition);
                if(Vector3.Distance(transform.position, startPosition) <= 0.1f)
                {
                    _isMovingIn = false;
                    _isMovingTowardsPlayer = true;
                }
                return;
            }
            
            float z = transform.position.z;

            if (z <= wall.despawnPosition.z)
            {
                _wallManager.RemoveWall(gameObject);
                return;
            }

            if (z <= player.transform.position.z && !_isMovingAway)
            {
                print($"Reached player");
                
                endPosition = new Vector3(wall.despawnPosition.x, wall.despawnPosition.y, wall.despawnPosition.z);
                
                switch (wall.wallDirection)
                {
                    case WallDirection.UP:
                        endPosition.y = wall.despawnPosition.y + 20;
                        break;
                    case WallDirection.DOWN:
                        endPosition.y = wall.despawnPosition.y - 20;
                        break;
                    case WallDirection.LEFT:
                        endPosition.x = wall.despawnPosition.x - 20;
                        break;
                    case WallDirection.RIGHT:
                        endPosition.x = wall.despawnPosition.x + 20;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _gameData.IncreaseScore(wall.ScoreToAdd);
                _isMovingAway = true;
                _isMovingTowardsPlayer = false;
            }

            if (_isMovingAway)
                MoveWallTowards(endPosition);
            
            else if (_isMovingTowardsPlayer)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 5f);
                MoveWallTowards(targetPosition);
            }
        }

        private void MoveWallTowards(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position,  position, Time.deltaTime * wall.speed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerMovement?.onWallHit.Invoke();
                _wallManager.RemoveWall(gameObject);
            }
        }
    }
}
