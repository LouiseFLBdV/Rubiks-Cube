using RubicksCubeCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private float rotateCooldown = 3;
    [SerializeField] private LayerMask m_InputLayerMask = Physics.DefaultRaycastLayers;
    
    private Cube m_Cube;
    private Camera m_Camera;

    

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
    
    void Start()
    {
        m_Camera = FindObjectOfType<Camera>();
        m_Cube = FindObjectOfType<Cube>();
        if (m_Cube)
        {
            InvokeRepeating("InvokeRandomMove", 0f, rotateCooldown);
        }
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CubePlayButton();
        }
    }
    
    void InvokeRandomMove()
    {
        if (m_Cube != null && m_Cube.gameObject.activeInHierarchy)
        {
            m_Cube.PlayRandomMove();
        }
    }

    void OnDestroy()
    {
        CancelInvoke("InvokeRandomMove"); // Остановка повторяющегося вызова при уничтожении объекта
    }
}
