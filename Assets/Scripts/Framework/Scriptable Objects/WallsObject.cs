using Framework.Enums;
using UnityEngine;

namespace Framework.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "WallsObject", menuName = "Walls/WallsObject")]
    public class WallsObject : ScriptableObject
    {
        public GameObject wall;
        [Range(1,10)] public float speed;
        public Vector3 spawnPosition;
        public Vector3 despawnPosition;
        public WallDirection wallDirection;
    }
}
