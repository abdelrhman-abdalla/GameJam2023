using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Animator deathScreenAnimator;
    [SerializeField] private TextMeshProUGUI gemCountText;

    private PlayerStats m_PlayerStats;

    private void Awake()
    {
        m_PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        m_PlayerStats.OnPlayerDie += OnPlayerDie;
    }

    void Update()
    {
        healthBarSlider.value = m_PlayerStats.Health;
        gemCountText.text = m_PlayerStats.GetGemCount().ToString();
    }

    private void OnPlayerDie()
    {
        deathScreenAnimator.gameObject.SetActive(true);
        deathScreenAnimator.SetBool("isVisible", true);
    }
}
