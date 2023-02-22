using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject JoinLobbyInputUI;
    [SerializeField] private GameObject LobbyUI;

    public void JoinLobby()
    {
        LobbyUI.SetActive(false);
        JoinLobbyInputUI.SetActive(true);
    }
    
    public void CancelJoinLobby()
    {
        JoinLobbyInputUI.SetActive(false);
        LobbyUI.SetActive(true);
    }
}
