using System;
using Framework.Enums;
using Framework.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Walls
{
    public class WallsMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 despawnPosition;
        [SerializeField] private GameObject player;
        [SerializeField] private WallsObject  wallsObject;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            despawnPosition.y = transform.position.y;
            despawnPosition.x = transform.position.x;
        }

        private void Update()
        {
            MoveWall();
        }

        private void MoveWall()
        {
            float z =  transform.position.z;

            if (z <= despawnPosition.z)
            {
                Destroy(gameObject);
                print("destroyed");
            }

            if (z <= player.transform.position.z)
            {
                var tPosition = transform.position;
                switch (wallsObject.wallDirection)
                {
                    case WallDirection.UP:
                        break;
                    case WallDirection.DOWN:
                        tPosition.y -= 20;
                        transform.position = Vector3.MoveTowards(transform.position,  tPosition, Time.deltaTime * speed);
                        break;
                    case WallDirection.LEFT:
                        break;
                    case WallDirection.RIGHT:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            transform.position = Vector3.MoveTowards(transform.position, despawnPosition, speed * Time.deltaTime);
        }
    }


}
