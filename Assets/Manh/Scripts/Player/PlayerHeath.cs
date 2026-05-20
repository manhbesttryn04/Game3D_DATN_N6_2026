using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // HEALTH
    // =========================

    public float maxHealth = 100f;
    public float currentHealth;

    // =========================
    // STATES
    // =========================

    public bool isDead;

    // =========================
    // UNITY
    // =========================

    void Start()
    {
        currentHealth = maxHealth;
    }

    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // =========================================================
    // HEAL
    // =========================================================

    public void Heal(float healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;

        currentHealth =
            Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // =========================================================
    // DIE
    // =========================================================

    void Die()
    {
        isDead = true;

        player.playerMove.isMove = false;

        player.playerAttack.canAttack = false;

        player.playerAnimator.playerAni.SetTrigger("Die");
    }

    // =========================================================
    // RESET
    // =========================================================

    public void ResetHealth()
    {
        isDead = false;

        currentHealth = maxHealth;

        player.playerMove.isMove = true;

        player.playerAttack.canAttack = true;
    }
}