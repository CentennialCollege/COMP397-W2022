using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlaneController : MonoBehaviour
{
    public Transform playerReSpawn;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ReSpawnPlayer(other.gameObject);
        }
    }

    public void ReSpawnPlayer(GameObject player)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = playerReSpawn.position;
        player.GetComponent<CharacterController>().enabled = true;
    }
}
