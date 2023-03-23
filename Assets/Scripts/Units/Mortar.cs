using UnityEngine;

public class Mortar : Tower
{
    private void Update()
    {
    }

    protected override void UpdateTarget()
    {
        Debug.Log("Mortar overrired");
    }
}
