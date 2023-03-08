using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildmanager;

    public void Start()
    {
        buildmanager = BuildManager.instance;
    }

    public void ChangeUnit(int index)
    {
        buildmanager.ChangeUnit(index);
    }
}