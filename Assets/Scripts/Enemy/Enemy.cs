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
    public Image slowImage;
    public GameObject damagePopUp;

    public GameObject dieEffect;
    public bool isDead;

    // Slowness
    private float currentSpeed;
    private bool slowed;

    private float slowedTime;

    // Start is called before the first frame update
    void Start()
    {
        slowImage.enabled = false;
        isDead = false;
        target = Waypoints.getWaypoints()[0];
        // Issisaugom gyvybes kiekvienam objektui.
        startHealh = (int)blueprint.health;
        health = startHealh;
        currentSpeed = blueprint.speed;
        slowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        SlowEffect();
    }

    private void Movement()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * currentSpeed * Time.deltaTime, Space.World);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavePointIndex >= Waypoints.getWaypoints().Length - 1 && !isDead)
        {
            isDead = true;
            PlayerStats.CastleHealth--;
            Die();
            return;
        }

        wavePointIndex++;
        target = Waypoints.getWaypoints()[wavePointIndex];
    }


    public void TakeDamage(float damage)
    {
        GameObject popUp = Instantiate(damagePopUp, transform.position, Quaternion.identity);
        popUp.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        Destroy(popUp, 1f);

        health -= damage;
        healthBar.fillAmount = health / startHealh;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        WaveSpawner.enemiesCount--;
        PlayerStats.EARNED_COINS += blueprint.coins;
        PlayerStats.Money += (int)blueprint.givesMoney;
        DieEffect();
    }


    private void DieEffect()
    {
        GameObject obj = Instantiate(dieEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1f);
    }

    public void TakeSlowness(float slow)
    {
        if (!slowed)
        {
            slowed = true;
            currentSpeed -= blueprint.speed * (1f - slow);
            slowedTime = Time.realtimeSinceStartup;
        }
    }

    private void SlowEffect()
    {
        if (slowed && slowImage.enabled == false)
        {
            slowImage.enabled = true;
            // Do countdown or something
        }

        // How long slow effects.
        if (Time.realtimeSinceStartup - slowedTime > 1.5f)
        {
            slowed = false;
            currentSpeed = blueprint.speed;
            slowImage.enabled = false;
        }
    }
}