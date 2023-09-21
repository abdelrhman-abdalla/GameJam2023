using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private float m_Health = 100.0f;

    public float Health
    {
        get { return m_Health; }
        private set { m_Health = value; }
    }

    public delegate void PlayerDieAction();
    public event PlayerDieAction OnPlayerDie;

    private const float DamageCooldown = 2;
    private float m_LastDamageTakenTime = -1000;

    public void ApplyDamages(float damage)
    {
        if (m_Health <= 0 || damage == 0)
            return;

        if (Time.time - m_LastDamageTakenTime <= DamageCooldown)
            return;

        m_Health -= damage;
        m_LastDamageTakenTime = Time.time;

        if (m_Health <= 0)
        {
            m_Health = 0;
            Kill();
        }
        else
        {
            StartCoroutine(DamageAnimation(10));
        }
    }

    public void Kill()
    {
        m_Health = 0;

        Debug.Log("Player die.");

        if (OnPlayerDie != null)
            OnPlayerDie();

        StartCoroutine(DelayedRestart(3f));
    }

    public bool IsAlive()
    {
        return m_Health > 0;
    }

    private IEnumerator DelayedRestart(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Reload current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator DamageAnimation(float duration)
    {
        float startTime = Time.time;

        while(Time.time - startTime < duration)
        {
            Color color = playerSpriteRenderer.color;
            color.a = (Mathf.Sin(Time.time * 10) + 2) / 3;
            playerSpriteRenderer.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
