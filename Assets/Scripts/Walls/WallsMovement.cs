using System;
using Data;
using Framework.Enums;
using Framework.Scriptable_Objects;
using UnityEngine;

namespace Walls
{
    public class WallsMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private WallsObject  wall;
        
        private GameObject _managerObject;
        private WallManager _wallManager;
        private GameData _gameData;
        private bool _isMovingAway;
        private Vector3 endPosition;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            _managerObject = GameObject.FindWithTag("Manager");
            _gameData = _managerObject.GetComponent<GameData>();
            _wallManager = _managerObject.GetComponent<WallManager>();
            wall.despawnPosition.y = transform.position.y;
            wall.despawnPosition.x = transform.position.x;
            endPosition = wall.despawnPosition;
        }

        private void FixedUpdate() => MoveWall();

        private void MoveWall()
        {
            if (!player.activeInHierarchy)
                return;
            
            float z =  transform.position.z;

            if (z <= wall.despawnPosition.z)
            {
                _wallManager.RemoveWall(gameObject);
                Destroy(gameObject);
            }

            if (z <= player.transform.position.z && !_isMovingAway)
            {
                switch (wall.wallDirection)
                {
                    case WallDirection.UP:
                        endPosition.y += 20;
                        break;
                    case WallDirection.DOWN:
                        endPosition.y -= 20;
                        break;
                    case WallDirection.LEFT:
                        endPosition.x -= 20;
                        break;
                    case WallDirection.RIGHT:
                        endPosition.x += 20;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _gameData.IncreaseScore(wall.ScoreToAdd);
                _isMovingAway = true;
            }
            MoveWallTowards(endPosition);
        }

        private void MoveWallTowards(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position,  position, Time.deltaTime * wall.speed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
