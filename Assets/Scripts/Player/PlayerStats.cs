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

    private int m_GemCount = 0;

    public delegate void PlayerDieAction();
    public event PlayerDieAction OnPlayerDie;

    private const float DamageCooldown = 2;
    private float m_LastDamageTakenTime = -1000;

    private void Update()
    {
        if (IsInvincibleCooldown() == true)
        {
            Color color = playerSpriteRenderer.color;
            color.a = (Mathf.Sin(Time.time * 30) + 2) / 3;
            playerSpriteRenderer.color = color;
        }
        else
        {
            playerSpriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    public void ApplyDamages(float damage)
    {
        if (m_Health <= 0 || damage == 0)
            return;

        if (IsInvincibleCooldown() == true)
            return;

        m_Health -= damage;
        m_LastDamageTakenTime = Time.time;

        if (m_Health <= 0)
        {
            m_Health = 0;
            Kill();
        }
    }

    public void Kill()
    {
        m_Health = 0;

        Debug.Log("Player die.");

        SceneManager.LoadScene(2);

        /*if (OnPlayerDie != null)
            OnPlayerDie();*/
    }

    public bool IsAlive()
    {
        return m_Health > 0;
    }

    public bool IsInvincibleCooldown()
    {
        return Time.time - m_LastDamageTakenTime <= DamageCooldown;
    }

    public void AddGem()
    { 
        m_GemCount++; 
    }

    public int GetGemCount()
    {
        return m_GemCount;
    }
}
