using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text inputLobbyCode;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private GameObject LobbyUI;
    private Lobby hostLobby;
    private Lobby joinedLobby;

    async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in playerID: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        
    }

    private async void CreateLobby(bool isPrivate)
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                IsPrivate = isPrivate,
                Player = CreatePlayer()
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            hostLobby = lobby;
            joinedLobby = lobby;
            InvokeRepeating("HandleLobbyHeartbeat", 15, 15);
            Debug.Log("Lobby created: " + lobby.Id);
            LobbyUI.SetActive(false);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("LobbyException: " + e.Message);
        }
        
    }
    
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions()
            {
                Player = CreatePlayer()
            };
            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            Debug.Log("Joined lobby: " + lobbyCode);
            LobbyUI.SetActive(false);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("LobbyException: " + e.Message);
        }
    }
    
    private async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions()
            {
                Player = CreatePlayer()
            };
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            LobbyUI.SetActive(false);

        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("LobbyException: " + e.Message);
        }
    }
    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby != null)
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            Debug.Log("Heartbeat sent");
        }

    }
    
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("LobbyException: " + e.Message);
        }
    }
    
    private Player CreatePlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
            {
                {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName.text)}
            }
        };
    }
    public void OnHostPrivateButtonClicked()
    {
        CreateLobby(true);
    }
    public void OnHostPublicButtonClicked()
    {
        CreateLobby(false);
    }
    public void OnJoinButtonClicked()
    {
        JoinLobbyByCode(inputLobbyCode.text);
    }
    public void OnQuickJoinButtonClicked()
    {
        QuickJoinLobby();
    }
    
    
}
