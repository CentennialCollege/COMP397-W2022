using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject localCamera;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    public void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
            TurnOffCamera();
        }

        //if (GUILayout.Button("Server"))
        //{
        //    NetworkManager.Singleton.StartServer();
        //}

        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
            TurnOffCamera();
        }
    }

    public void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        //GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    public void TurnOffCamera()
    {
        canvas.SetActive(true);
        localCamera.SetActive(false);
    }

    public void TurnOnCamera()
    {
        canvas.SetActive(false);
        localCamera.SetActive(true);
    }

}
