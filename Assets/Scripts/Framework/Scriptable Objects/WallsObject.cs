using UnityEngine;

namespace Framework.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "WallsObject", menuName = "Walls/WallsObject")]
    public class WallsObject : ScriptableObject
    {
        public GameObject Wall;
        [Range(1,10)] public float Speed;
    }
}
