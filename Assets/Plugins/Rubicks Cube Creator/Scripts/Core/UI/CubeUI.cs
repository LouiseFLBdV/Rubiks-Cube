using UnityEngine;
using static RubicksCubeCreator.CubeUtility;

namespace RubicksCubeCreator
{
    public class CubeUI : MonoBehaviour
    {
        [SerializeField] private Cube m_Cube;
        [SerializeField] private CubeFaceUI m_TopFace;
        [SerializeField] private CubeFaceUI m_FrontFace;
        [SerializeField] private CubeFaceUI m_LeftFace;
        [SerializeField] private CubeFaceUI m_RightFace;
        [SerializeField] private CubeFaceUI m_BottomFace;
        [SerializeField] private CubeFaceUI m_BackFace;

        private void OnEnable()
        {
            if(m_Cube == null)
            {
                return;
            }
            if(m_Cube.Preset.Dimension * m_Cube.Preset.Dimension != m_TopFace.transform.childCount)
            {
                Debug.LogWarning("Top face has wrong number of children, closing", this);
                gameObject.SetActive(false);
                return;
            }
            m_Cube.OnCubeChanged.AddListener(UpdateFaces);
        }
        private void OnDisable()
        {
            if (m_Cube == null)
            {
                return;
            }
            m_Cube.OnCubeChanged.RemoveListener(UpdateFaces);
        }
        private void Start()
        {
            UpdateFaces();
        }
        private void UpdateFaces()
        {
            if(m_Cube == null)
            {
                Debug.LogWarning("Cube is null", this);
                return;
            }
            m_TopFace.SetColors(m_Cube.GetColorsOfFace(Sides.Top));
            m_FrontFace.SetColors(m_Cube.GetColorsOfFace(Sides.Front));
            m_LeftFace.SetColors(m_Cube.GetColorsOfFace(Sides.Left));
            m_RightFace.SetColors(m_Cube.GetColorsOfFace(Sides.Right));
            m_BottomFace.SetColors(m_Cube.GetColorsOfFace(Sides.Bottom));
            m_BackFace.SetColors(m_Cube.GetColorsOfFace(Sides.Back));
        }
    }
}
