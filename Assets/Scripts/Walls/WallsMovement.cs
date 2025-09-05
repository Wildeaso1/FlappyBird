using System;
using Framework.Enums;
using Framework.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Walls
{
    public class WallsMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private WallsObject  wall;

        private GameObject _manager;
        private WallManager _wallManager;
        private bool _isMovingAway;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            wall.despawnPosition.y = transform.position.y;
            wall.despawnPosition.x = transform.position.x;
        }

        private void FixedUpdate()
        {
            MoveWall();
        }

        private void MoveWall()
        {
            float z =  transform.position.z;

            if (z <= wall.despawnPosition.z)
            {
                Destroy(gameObject);
                print("destroyed");
            }

            if (z <= player.transform.position.z && !_isMovingAway)
            {
                switch (wall.wallDirection)
                {
                    case WallDirection.UP:
                        wall.despawnPosition.y += 20;
                        break;
                    case WallDirection.DOWN:
                        wall.despawnPosition.y -= 20;
                        break;
                    case WallDirection.LEFT:
                        wall.despawnPosition.x -= 20;
                        break;
                    case WallDirection.RIGHT:
                        wall.despawnPosition.x += 20;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _isMovingAway = true;
            }
            MoveWallTowards(wall.despawnPosition);
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
