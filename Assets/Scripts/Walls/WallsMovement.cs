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
                        break;
                    case WallDirection.DOWN:
                        tPosition.y -= 20;
                        transform.position = Vector3.MoveTowards(transform.position,  tPosition, Time.deltaTime * wall.speed);
                        break;
                    case WallDirection.LEFT:
                        break;
                    case WallDirection.RIGHT:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            transform.position = Vector3.MoveTowards(transform.position, wall.despawnPosition, wall.speed * Time.deltaTime);
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
