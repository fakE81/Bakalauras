using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName="PathCell", menuName = "TowerDefence/PathCell")]
public class PathCellObject : ScriptableObject
{
    public EndpointNeigbourValue downValue;
    public EndpointNeigbourValue upValue;
    public EndpointNeigbourValue leftValue;
    public EndpointNeigbourValue rightValue;
}
