using System.IO;
using UnityEngine;

public class PlayerTowersManager : MonoBehaviour
{
    [SerializeField] private UnitBlueprint[] unitBlueprints;
    public static PlayerTowersManager instance;
    public AudioSource audioSource;

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
            // Play Audio
            if(!audioSource.isPlaying){
                audioSource.Play();
            }
        }
    }

    public void SaveData()
    {
        string balista = JsonUtility.ToJson(unitBlueprints[0].prefab.transform.GetChild(0).GetComponent<Balista>()
            .TowerInformation);
        if (!Directory.Exists(Application.persistentDataPath + "/Towers"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Towers");
        }

        File.WriteAllText(Application.persistentDataPath + "/Towers/Balista.json", balista);
    }

    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Towers/Balista.json"))
        {
            string balista = File.ReadAllText(Application.persistentDataPath + "/Towers/Balista.json");
            TowerInformation towerInformation = JsonUtility.FromJson<TowerInformation>(balista);
            unitBlueprints[0].prefab.transform.GetChild(0).GetComponent<Balista>().TowerInformation = towerInformation;
            Debug.Log(towerInformation);
        }
    }
}