using UnityEngine;

public class RayCastMenu : MonoBehaviour
{
    [SerializeField] private LayerMask m_InputLayerMask = Physics.DefaultRaycastLayers;
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CubePlayButton();
        }
    }

    public void CubePlayButton()
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, m_InputLayerMask))
        {
            if (hit.collider.CompareTag("RubicksCube"))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
