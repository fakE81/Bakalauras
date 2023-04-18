using System.IO;
using UnityEngine;

public class PlayerTowersManager : MonoBehaviour
{
    [SerializeField] private UnitBlueprint[] unitBlueprints;
    public static PlayerTowersManager instance;

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
        bool leveledUp = unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Tower>().LevelUp();
        if(leveledUp){
            MusicManager.instance.PlayLevelUp();
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

        File.WriteAllText(Application.persistentDataPath + "/Towers/Balista.json", balista);
        File.WriteAllText(Application.persistentDataPath + "/Towers/Mortar.json", mortar);
    }

    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Towers/Balista.json"))
        {
            string balista = File.ReadAllText(Application.persistentDataPath + "/Towers/Balista.json");
            TowerInformation towerInformation = JsonUtility.FromJson<TowerInformation>(balista);
            unitBlueprints[0].prefab.transform.GetChild(0).GetComponent<Balista>().TowerInformation = towerInformation;
        }

        if (File.Exists(Application.persistentDataPath + "/Towers/Mortar.json"))
        {
            string mortar = File.ReadAllText(Application.persistentDataPath + "/Towers/Mortar.json");
            TowerInformation towerInformation = JsonUtility.FromJson<TowerInformation>(mortar);
            unitBlueprints[1].prefab.transform.GetChild(0).GetComponent<Mortar>().TowerInformation = towerInformation;  
        }
    }
}