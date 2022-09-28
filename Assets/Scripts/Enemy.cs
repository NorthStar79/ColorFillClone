using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject EnemyFructure;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(!other.gameObject.GetComponent<PlayerController>().isAlive) return;
            
            other.gameObject.GetComponent<PlayerController>().Die();

            Die();
        }
    }

    public void Die()
    {
        Instantiate(EnemyFructure, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

}
