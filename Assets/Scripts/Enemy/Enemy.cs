using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Stats skirti butent vienai kategorijai priesu.
    public EnemyBlueprint blueprint;
    private Transform target;
    private int wavePointIndex = 0;

    private float health;
    private int startHealh;

    // Health bar padaryti kad nesisuktu.
    public Image healthBar;

    public GameObject dieEffect;

    // Start is called before the first frame update
    void Start()
    {
        target = Waypoints.getWaypoints()[0];
        // Issisaugom gyvybes kiekvienam objektui.
        startHealh = (int)blueprint.health;
        health = startHealh;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement:
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * blueprint.speed * Time.deltaTime, Space.World);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        Debug.Log(wavePointIndex + " >=" + Waypoints.getSize());
        if (wavePointIndex >= Waypoints.getWaypoints().Length - 1)
        {
            // Jei pasiekiam mazinam gyvybes.
            PlayerStats.CastleHealth--;
            Die();
            return; // Nes uztrunka istrinti
        }

        wavePointIndex++;
        target = Waypoints.getWaypoints()[wavePointIndex];
    }


    public void TakeDamage(float damage)
    {
        health -= damage;

        // Healthbarui.
        healthBar.fillAmount = health / startHealh;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        WaveSpawner.enemiesCount--;
        PlayerStats.Money += (int)blueprint.givesMoney;
        DieEffect();
    }


    private void DieEffect()
    {
        GameObject obj = Instantiate(dieEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
    }
}