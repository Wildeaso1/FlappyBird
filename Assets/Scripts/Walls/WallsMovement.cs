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

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            wall.despawnPosition.y = transform.position.y;
            wall.despawnPosition.x = transform.position.x;
        }

        private void Update()
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

            if (z <= player.transform.position.z)
            {
                var tPosition = transform.position;
                switch (wall.wallDirection)
                {
                    case WallDirection.UP:
                        tPosition.y += 20;
                        break;
                    case WallDirection.DOWN:
                        tPosition.y -= 20;
                        break;
                    case WallDirection.LEFT:
                        tPosition.x -= 20;
                        break;
                    case WallDirection.RIGHT:
                        tPosition.x += 20;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                MoveWallTowards(tPosition);
            }
            
            transform.position = Vector3.MoveTowards(transform.position, wall.despawnPosition, wall.speed * Time.deltaTime);
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
