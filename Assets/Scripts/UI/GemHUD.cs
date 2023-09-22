using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemHUD : MonoBehaviour
{
    [SerializeField] private GameObject hudElement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.AddGem();
                Destroy(hudElement);
                Destroy(gameObject);
            }
        }
    }
}
