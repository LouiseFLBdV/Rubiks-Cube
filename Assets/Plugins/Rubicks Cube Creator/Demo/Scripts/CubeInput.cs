using UnityEngine;
using static RubicksCubeCreator.CubeUtility;

namespace RubicksCubeCreator.Demo
{
    public class CubeInput : MonoBehaviour
    {
        [SerializeField] private LayerMask m_InputLayerMask = Physics.DefaultRaycastLayers;
        [SerializeField] private bool m_InputGet = true;
        [SerializeField] private float inputGap = 0.1f;
        public bool InputGet
        {
            get => m_InputGet;
            set => m_InputGet = value;
        }
        private Cube m_RubicksCube;
        private RaycastHit m_Hit;
        [SerializeField] private Camera m_Camera;

        private Vector3 m_MouseHitStart;
        private Block m_Block;

        private void Awake()
        {
            if (m_Camera == null)
                m_Camera = Camera.main;
        }

        private void Update()
        {
            if (!InputGet || !m_Camera) return;

            if (Input.GetMouseButtonDown(0))
            {
                SelectBlock();
            }
            if (Input.GetMouseButton(0))
            {
                CalculateMove();
            }
            if (Input.GetMouseButtonUp(0))
            {
                ResetInput();
            }
        }
        private bool GetHitFromCamera(out RaycastHit hit)
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f, m_InputLayerMask))
            {
                return true;
            }
            return false;
        }
        private void SelectBlock()
        {
            if (GetHitFromCamera(out m_Hit))
            {
                if (m_RubicksCube == null)
                    m_RubicksCube = m_Hit.collider.GetComponent<Cube>();

                m_Block = m_RubicksCube.GetBlockOnThePoint(m_Hit.point);
                m_MouseHitStart = m_Hit.point;
            }
        }
        private void CalculateMove()
        {
            if (m_Block == null)
                return;

            if (GetHitFromCamera(out var currentHit))
            {
                if (Vector3.Distance(m_MouseHitStart, currentHit.point) > inputGap)
                {
                    Vector3 direction = m_MouseHitStart - currentHit.point;
                    Sides hittedSide = m_RubicksCube.GetClosestSideWithNormal(m_RubicksCube.transform.TransformPoint(m_RubicksCube.transform.position - m_MouseHitStart.normalized));
                    Sides towards = m_RubicksCube.GetClosestSideWithNormal(m_RubicksCube.transform.TransformPoint(direction));
                    m_RubicksCube.PlayMove(hittedSide, towards, m_Block);
                    ResetInput();
                }
            }
        }
        private void OnDrawGizmos()
        {
            if (m_Block == null)
                return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_MouseHitStart, 0.1f);
            if (GetHitFromCamera(out var hit))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.1f);
                Gizmos.DrawLine(m_MouseHitStart, hit.point);
            }
        }
        private Vector3 GetRawNormal(Vector3 normal)
        {
            if (normal == Vector3.zero) return Vector3.zero;
            Vector3 rawNormal = Vector3.zero;
            float max = Mathf.Max(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));
            if (Mathf.Abs(normal.x) == max)
                rawNormal = new Vector3(Mathf.Sign(normal.x), 0, 0);
            else if (Mathf.Abs(normal.y) == max)
                rawNormal = new Vector3(0, Mathf.Sign(normal.y), 0);
            else if (Mathf.Abs(normal.z) == max)
                rawNormal = new Vector3(0, 0, Mathf.Sign(normal.z));

            return rawNormal;
        }
        private void ResetInput()
        {
            m_Block = null;
        }
    }
}
