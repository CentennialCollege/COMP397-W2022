using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerBehaviour : NetworkBehaviour
{
    public CharacterController controller;

    [Header("Movement")]
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;
    public Vector3 velocity;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;
    public bool isGrounded;

    private NetworkVariable<float> remoteVerticalInput = new NetworkVariable<float>();
    private NetworkVariable<float> remoteHorizontalInput = new NetworkVariable<float>();
    private NetworkVariable<bool> remoteJumpInput = new NetworkVariable<bool>();

    private float localHorizontalInput;
    private float localVerticalInput;
    private bool localJumpInput;
  

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsLocalPlayer)
        {
            controller = GetComponent<CharacterController>();
        }

        RandomSpawnPositionAndColour();
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

    private void Move()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f && IsLocalPlayer)
        {
            velocity.y = -2.0f;
        }

        // keyboard Input (fallback)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isJumping = Input.GetButton("Jump");


        if (isJumping && isGrounded && IsLocalPlayer)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        if (IsLocalPlayer)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
        // check if local variables have changed
        if (localHorizontalInput != x || localVerticalInput != z || localJumpInput != isJumping)
        {
            localHorizontalInput = x;
            localVerticalInput = z;
            localJumpInput = isJumping;

            // update the client position on the network
            UpdateClientPositionServerRpc(x, z, isJumping);
        }
    }

    void ServerUpdate()
    {
        Vector3 move = transform.right * remoteHorizontalInput.Value + transform.forward * remoteVerticalInput.Value;
        controller.Move(move * maxSpeed * Time.deltaTime);
    }

    public void RandomSpawnPositionAndColour()
    {
        var x = Random.Range(-3.0f, 3.0f);
        var z = Random.Range(-3.0f, 3.0f);
        transform.position = new Vector3(x, 1.0f, z);
    }

    public void ClientUpdate()
    {
        Move();
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float horizontal, float vertical, bool isJumping)
    {
        remoteHorizontalInput.Value = horizontal;
        remoteVerticalInput.Value = vertical;
        remoteJumpInput.Value = isJumping;
    }

}
