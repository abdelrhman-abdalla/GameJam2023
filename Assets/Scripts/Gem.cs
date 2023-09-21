using UnityEngine;

public class Gem : MonoBehaviour
{
    bool m_Used = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Used == true)
            return;

        if (collision.tag == "Player")
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.AddGem();
                m_Used = true;
                Destroy(gameObject);
            }
        }
    }
}
