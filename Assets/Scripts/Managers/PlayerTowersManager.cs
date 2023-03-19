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
    }

    public UnitBlueprint[] UnitBlueprints
    {
        get => unitBlueprints;
        set => unitBlueprints = value;
    }

    public void ChangeBlueprintBasicPrice()
    {
        
    }

    public void LevelUp(int index)
    {
        unitBlueprints[index].prefab.transform.GetChild(0).GetComponent<Balista>().LevelUp();
    }
}
