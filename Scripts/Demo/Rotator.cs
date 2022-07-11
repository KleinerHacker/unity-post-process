#if DEBUG
using UnityEngine;

namespace UnityPostProcess.Demo.post_process.Scripts.Demo
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1f;

        private void FixedUpdate()
        {
            transform.rotation *= Quaternion.Euler(0f, Time.fixedDeltaTime * speed, 0f);
        }
    }
}
#endif