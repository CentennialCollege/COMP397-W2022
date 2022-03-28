using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerBehaviour : NetworkBehaviour
{
    public float speed;

    private NetworkVariable<float> verticalPosition = new NetworkVariable<float>();
    private NetworkVariable<float> horizontalPosition = new NetworkVariable<float>();

    private float localHorizontal;
    private float localVertical;

    // Start is called before the first frame update
    void Start()
    {
        RandomSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            // server update
            ServerUpdate();
        }

        if (IsClient && IsOwner)
        {
            // client update
            ClientUpdate();
        }
    }

    void ServerUpdate()
    {
        transform.position = new Vector3(transform.position.x + horizontalPosition.Value,
            transform.position.y, transform.position.z + verticalPosition.Value);
    }

    public void RandomSpawnPosition()
    {
        var x = Random.Range(-10.0f, 10.0f);
        var z = Random.Range(-10.0f, 10.0f);
        transform.position = new Vector3(x, 1.0f, z);
    }

    public void ClientUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        // network update
        if (localHorizontal != horizontal || localVertical != vertical)
        {
            localHorizontal = horizontal;
            localVertical = vertical;
            // update the client position on the network
            UpdateClientPositionServerRpc(horizontal, vertical);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float horizontal, float vertical)
    {
        horizontalPosition.Value = horizontal;
        verticalPosition.Value = vertical;
    }
}
