using UnityEngine;

namespace RubicksCubeCreator.Demo
{
    public class RotateObjectInput : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                m_Target.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * 10f, Space.World);
                m_Target.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * 10f, Space.World);
            }
        }
    }
}
