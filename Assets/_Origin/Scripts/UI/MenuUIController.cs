using System;
using System.Collections.Generic;
using RubicksCubeCreator;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private float rotateCooldown = 3;
    [SerializeField] private LayerMask m_InputLayerMask = Physics.DefaultRaycastLayers;
    [SerializeField] private Text userName;
    [SerializeField] private Text money;
    [SerializeField] private List<GameObject> popupList;
    private Cube m_Cube;
    private Camera m_Camera;

    private void Start()
    {
        m_Camera = FindObjectOfType<Camera>();
        m_Cube = FindObjectOfType<Cube>();
        if (m_Cube)
        {
            InvokeRepeating("InvokeRandomMove", 0f, rotateCooldown);
        }
        SaveManager.Instance.onLoadedSaveData.AddListener(RenderMoney);
        PlayerData.Instance.onPlayerMoneyChanged.AddListener(RenderMoney);
    }

    private void RenderMoney()
    {
        money.text = PlayerData.Instance.Money.ToString();
    }
    
    void InvokeRandomMove()
    {
        if (m_Cube != null && m_Cube.gameObject.activeInHierarchy)
        {
            m_Cube.PlayRandomMove();
        }
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
                GameManager.Instance.ChangeGameState(GameManager.GameState.Game);
            }
        }
    }

    public void ChangeUserName()
    {
        if (PlayerData.Instance.UserName != userName.text)
        {
            PlayerData.Instance.UserName = userName.text;
        }
    }

    public void SelectPopup(GameObject gameObject)
    {
        foreach (var popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
        m_Cube.gameObject.SetActive(false);
    }

    public void ClosePopup()
    {
        foreach (var popup in popupList)
        {
            popup.gameObject.SetActive(false);
        }

        m_Cube.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        CancelInvoke("InvokeRandomMove");
        SaveManager.Instance.onLoadedSaveData.RemoveListener(RenderMoney);
        PlayerData.Instance.onPlayerMoneyChanged.RemoveListener(RenderMoney);
    }
}