using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public UIControls controls;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controls.TakeDamage(10);
        }

        if(other.gameObject.CompareTag("Hazard"))
        {
            controls.TakeDamage(20);
        }
        
    }
    
}
