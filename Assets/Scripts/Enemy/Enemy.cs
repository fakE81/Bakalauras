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
    public GameObject damagePopUp;

    public GameObject dieEffect;
    public bool isDead;

    //DEBUG:
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private Vector3 velocity;
    private Vector3 futurePosition;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        target = Waypoints.getWaypoints()[0];
        // Issisaugom gyvybes kiekvienam objektui.
        startHealh = (int)blueprint.health;
        health = startHealh;


        // DEBUG:
        currentPosition = transform.position;
        previousPosition = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate velocity using the change in position over time
        currentPosition = transform.position;
        velocity = (currentPosition - previousPosition) / Time.deltaTime;
        previousPosition = currentPosition;

        // Calculate future position based on current position and velocity
        float time = 3f; // Time in seconds to predict the future position
        futurePosition = currentPosition + velocity * time;
        Debug.DrawLine(currentPosition, futurePosition, Color.green);


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
        // Debug.Log(wavePointIndex + " >=" + Waypoints.getSize());
        if (wavePointIndex >= Waypoints.getWaypoints().Length - 1 && !isDead)
        {
            // Jei pasiekiam mazinam gyvybes.
            isDead = true;
            PlayerStats.CastleHealth--;
            Die();
            return; // Nes uztrunka istrinti
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
}