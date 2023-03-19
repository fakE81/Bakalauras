using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    private UnitBlueprint selectedBlueprint;
    [SerializeField] private UnitBlueprint[] unitBlueprints;
    [SerializeField] private GameObject[] selectPanels;
    private GameObject currentSelectedPanel;
    public int currentSelectedUnit;
    public bool buildingMode = false;
    [SerializeField] private Text balistaPriceText;
    

    public bool HasMoney
    {
        get { return PlayerStats.Money >= selectedBlueprint.cost; }
    }

    void Awake()
    {
        unitBlueprints = PlayerTowersManager.instance.UnitBlueprints;
        selectedBlueprint = null;
        currentSelectedPanel = null;
        currentSelectedUnit = -1;
        if (instance != null)
        {
            return;
        }

        instance = this;
    }


    private void Update()
    {
        SelectUnit();
    }

    private void SelectUnit()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeUnit(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeUnit(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeUnit(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeUnit(3);
        }
    }

    public UnitBlueprint getCurrentBlueprint()
    {
        return selectedBlueprint;
    }

    public void ChangeUnit(int index)
    {
        ChangeSelectedPanel(index);
        currentSelectedUnit = index;
    }

    private void ChangeSelectedPanel(int index)
    {
        if (currentSelectedPanel == null)
        {
            currentSelectedPanel = selectPanels[index];
            currentSelectedPanel.SetActive(true);
            SelectUnit(index);
            buildingMode = true;
        }
        else if (currentSelectedUnit == index)
        {
            DeselectUnit();
            currentSelectedPanel.SetActive(false);
            currentSelectedPanel = null;
            currentSelectedUnit = -1;
            buildingMode = false;
        }
        else
        {
            currentSelectedPanel.SetActive(false);
            currentSelectedPanel = selectPanels[index];
            currentSelectedPanel.SetActive(true);
            SelectUnit(index);
            buildingMode = true;
        }
    }

    private void SelectUnit(int index)
    {
        selectedBlueprint = unitBlueprints[index];
    }

    private void DeselectUnit()
    {
        selectedBlueprint = null;
    }

    public Text getBalistaText()
    {
        return balistaPriceText;
    }
}