using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Animator deathScreenAnimator;

    private PlayerStats m_PlayerStats;

    private void Awake()
    {
        m_PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        m_PlayerStats.OnPlayerDie += OnPlayerDie;
    }

    void Update()
    {
        healthBarSlider.value = m_PlayerStats.Health;
    }

    private void OnPlayerDie()
    {
        deathScreenAnimator.gameObject.SetActive(true);
        deathScreenAnimator.SetBool("isVisible", true);
    }
}
