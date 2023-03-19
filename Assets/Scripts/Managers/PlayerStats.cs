using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    [SerializeField] private int startMoney = 400;

    public static int Wave = 0;

    public static int CastleHealth;
    public int startCastleHealth = 10;

    private GameManager gm;
    public static int EARNED_COINS = 0;

    void Start()
    {
        CastleHealth = startCastleHealth;
        Money = startMoney;
        Wave = 0;
        EARNED_COINS = 0;
        gm = GameManager.instance;
    }
}