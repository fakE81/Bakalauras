using System.IO;
using UnityEngine;

public class PlayerTowersManager : MonoBehaviour
{
    [SerializeField] private UnitBlueprint[] unitBlueprints;
    [SerializeField] private BaseTowerInformation[] baseTowerInformation;
    public static PlayerTowersManager instance;
    public int MAX_LEVEL = 20;

    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        ChangeBlueprintBasicPrice();
        LoadData();
    }

    public UnitBlueprint[] UnitBlueprints
    {
        get => unitBlueprints;
        set => unitBlueprints = value;
    }

    /**
     * Call this method before starting game to reset prices.
     */
    public void ChangeBlueprintBasicPrice()
    {
        foreach (var blueprint in unitBlueprints)
        {
            blueprint.cost = blueprint.BASE_COST;
        }
    }

    public void LevelUp(int index)
    {
        int currentLevel = unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Tower>().TowerInformation
            .level;
        if (currentLevel < MAX_LEVEL)
        {
            bool leveledUp = unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Tower>().LevelUp();
            if(leveledUp){
                MusicManager.instance.PlayLevelUp();
            }
        }
    }

    public void SaveData()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Towers"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Towers");
        }
        string balista = JsonUtility.ToJson(unitBlueprints[0].prefab.transform.GetChild(0).GetComponent<Tower>()
            .TowerInformation);
        string mortar = JsonUtility.ToJson(unitBlueprints[1].prefab.transform.GetChild(0).GetComponent<Tower>()
            .TowerInformation);
        string ice = JsonUtility.ToJson(unitBlueprints[2].prefab.transform.GetChild(0).GetComponent<Tower>()
            .TowerInformation);
        string mine = JsonUtility.ToJson(unitBlueprints[3].prefab.transform.GetChild(0).GetComponent<Tower>()
            .TowerInformation);

        File.WriteAllText(Application.persistentDataPath + "/Towers/Balista.json", balista);
        File.WriteAllText(Application.persistentDataPath + "/Towers/Mortar.json", mortar);
        File.WriteAllText(Application.persistentDataPath + "/Towers/Ice.json", ice);
        File.WriteAllText(Application.persistentDataPath + "/Towers/Mine.json", mine);
    }

    private void LoadData()
    {
        LoadTower("/Towers/Balista.json", 0);
        LoadTower("/Towers/Mortar.json", 1);
        LoadTower("/Towers/Ice.json", 2);
        LoadTower("/Towers/Mine.json", 3);
    }

    private void LoadTower(string path, int index)
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            string mortar = File.ReadAllText(Application.persistentDataPath + path);
            TowerInformation towerInformation = JsonUtility.FromJson<TowerInformation>(mortar);
            unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Tower>().TowerInformation = towerInformation;  
        }
        else
        {
            BaseTowerInformation information = baseTowerInformation[index];
            TowerInformation towerInformation = new TowerInformation();
            towerInformation.towerName = information.towerName;
            towerInformation.range = information.range;
            towerInformation.upgradeCost = information.upgradeCost;
            towerInformation.damage = information.damage;
            towerInformation.level = information.level;
            towerInformation.slow = information.slow;
            towerInformation.turnSpeed = information.turnSpeed;
            towerInformation.fireRate = information.fireRate;
            unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Tower>().TowerInformation = towerInformation;
        }
    }
}