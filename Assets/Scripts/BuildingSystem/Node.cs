using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private Renderer rend;
    private Material startMaterial;
    public Material hoverMaterial;
    public Material cantBuildMaterial;
    public bool hasMine;

    private GameObject unit;
    private UnitBlueprint blueprint;
    public float offset; // Kiek pakelti
    private BuildManager buildmanager;

    [SerializeField] private int groundType;

    // Start is called before the first frame update
    void Start()
    {
        hasMine = false;
        rend = GetComponentInChildren<Renderer>();
        startMaterial = rend.materials[1];
        buildmanager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //Building turret:
        if (unit != null)
        {
            //Reiskias turim lankininka ant sito tile
            return;
        }

        //Instantiate archer.
        buildUnit(buildmanager.getCurrentBlueprint(), buildmanager.currentSelectedUnit, buildmanager.getBalistaText(buildmanager.currentSelectedUnit));
    }

    void buildUnit(UnitBlueprint blueprint, int index, Text price)
    {
        if (blueprint == null)
            return;
        if (!CanBuild(index))
            return;
        if (PlayerStats.Money < blueprint.cost)
            return;

        PlayerStats.Money -= blueprint.cost;
        unit = Instantiate(blueprint.prefab,
            new Vector3(transform.position.x, transform.position.y + offset, transform.position.z),
            Quaternion.identity);
        blueprint.cost += blueprint.costIncrease;
        price.text = blueprint.cost + "$";
    }

    private bool CanBuild(int index)
    {
        if (groundType == 0 && index == 4)
            return false;
        if (groundType == 1 && (index == 0 || index == 1 || index == 2 || index == 3))
            return false;
        return true;
    }

    private void OnMouseEnter()
    {
        if (!buildmanager.buildingMode)
            return;
        Material[] mesh = rend.materials;
        if (CanBuild(buildmanager.currentSelectedUnit) && buildmanager.HasMoney)
        {
            mesh[1] = hoverMaterial;
        }
        else
        {
            mesh[1] = cantBuildMaterial;
        }

        rend.materials = mesh;
    }

    private void OnMouseExit()
    {
        Material[] mesh = rend.materials;
        if (!buildmanager.buildingMode)
            return;
        mesh[1] = startMaterial;
        rend.materials = mesh;
    }
}