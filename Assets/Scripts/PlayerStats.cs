using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    [SerializeField] private int startMoney = 400;

    public static int Wave = 0;

    [SerializeField] public static int CastleHealth;
    public int startCastleHealth = 10;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Pradiniai stats.
        CastleHealth = startCastleHealth;
        Money = startMoney;
        Wave = 0;
        gm = GameManager.instance;
    }
}