using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticsManager : MonoBehaviour
{
    public static PlayerStatisticsManager instance;

    [SerializeField]
    private Experience balistaExperience;
    void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        balistaExperience = new Experience();
        DontDestroyOnLoad(this.gameObject);
        // Load from the file balistaExperience:
    }

    public void AddExeperienceToBalista(int experience)
    {
        Debug.Log("Balista got exp:" + experience);
        balistaExperience.experience += experience;
    }

    public void HandleLevelUps()
    {
        while (balistaExperience.experience >= balistaExperience.neededExperience)
        {
            balistaExperience.experience -= balistaExperience.neededExperience;
            balistaExperience.neededExperience *= 2;
            balistaExperience.level++;
            balistaExperience.experiencePoints++;
        }
        Debug.Log("Balista level:" + balistaExperience.level);
    }

    // Do Saving:
    public void SaveToFileExperience()
    {
        
    }
}
