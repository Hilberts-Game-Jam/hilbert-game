using UnityEngine;

namespace Scripts.Player
{
    public class CameraFollow2D : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 0, -10);

        void LateUpdate()
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.identity;
        }
    }
}