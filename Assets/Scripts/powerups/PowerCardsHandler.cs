using UnityEngine;

public class PowerCardsHandler : MonoBehaviour
{
    public int money = 100;
    public int range = 1;
    public int damage = 10;
    public int speed;
    private Balista[] archers;

    public void handle(string powerCardName)
    {
        switch (powerCardName)
        {
            case "Money":
                PlayerStats.Money += money;
                break;
            case "Range":
                archers = FindObjectsOfType<Balista>();
                foreach (var item in archers)
                {
                    item.range += range;
                }

                break;
            case "Damage":
                archers = FindObjectsOfType<Balista>();
                Debug.Log(archers.Length);
                foreach (var item in archers)
                {
                    item.damage += damage;
                }

                break;
        }
    }
}