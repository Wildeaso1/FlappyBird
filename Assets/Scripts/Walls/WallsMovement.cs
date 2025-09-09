using System;
using Framework.Enums;
using Framework.Scriptable_Objects;
using UnityEngine;

namespace Walls
{
    public class WallsMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private WallsObject  wall;
        
        private GameObject _wallManagerObject;
        private WallManager _wallManager;
        private bool _isMovingAway;
        private Vector3 endPosition;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            _wallManagerObject = GameObject.FindWithTag("Wallmanager");
            _wallManager = _wallManagerObject.GetComponent<WallManager>();
            wall.despawnPosition.y = transform.position.y;
            wall.despawnPosition.x = transform.position.x;
            endPosition = wall.despawnPosition;
        }

        private void FixedUpdate() => MoveWall();

        private void MoveWall()
        {
            float z =  transform.position.z;

            if (z <= wall.despawnPosition.z)
            {
                _wallManager.RemoveWall(wall.wall);
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
                Destroy(player);
                Destroy(gameObject);
            }
        }
    }
}
