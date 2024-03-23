using UnityEngine;
using UnityEngine.UI;

namespace RubicksCubeCreator
{
    public class CubeFaceUI : MonoBehaviour
    {
        [SerializeField] private Image[] m_Images;
        /// <summary>
        /// Sets the color of the images
        /// </summary>
        /// <param name="colors"></param>
        public void SetColors(Color[] colors)
        {
            for (int i = 0; i < m_Images.Length; i++)
            {
                m_Images[i].color = colors[i];
            }
        }
    }
}
