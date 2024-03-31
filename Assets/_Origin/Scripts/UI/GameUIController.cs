using RubicksCubeCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class GameUIController : MonoBehaviour
{
    private Cube m_Cube;
    [SerializeField] private float rotateCooldown = 3;
    void Start()
    {
        m_Cube = FindObjectOfType<Cube>();
        if (m_Cube)
        {
            InvokeRepeating("InvokeRandomMove", 0f, rotateCooldown);
        }
    }
    
    void InvokeRandomMove()
    {
        m_Cube.PlayRandomMove();
    }

    void OnDestroy()
    {
        CancelInvoke("InvokeRandomMove"); // Остановка повторяющегося вызова при уничтожении объекта
    }
}
