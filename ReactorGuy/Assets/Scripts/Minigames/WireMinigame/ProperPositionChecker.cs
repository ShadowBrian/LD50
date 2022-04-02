using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProperPositionChecker : MonoBehaviour
{
    [SerializeField] private List<PossiblePositionsSO> datas;
    private PossiblePositionsSO currentData;
    private List<WirePosition> positions = new List<WirePosition>();
    public List<Collider> properWireColliders = new List<Collider>();

    private void Awake()
    {
        foreach(Transform t in transform)
        {
            positions.Add(t.GetComponent<WirePosition>());
        }
    }

    public void CheckPosition()
    {
        currentData = GetRandom();
        string chosen = null;
        (string a, string b) = currentData.GetPositions();
        chosen = Random.Range(0, 2) switch
        {
            0 => a,
            _ => b
        };

        foreach(var t in positions)
        {
            if(t.name.Contains(chosen))
            {
                t.TurnOn();
            }
            else
            {
                t.TurnOff();
            }
        }

        properWireColliders.Clear();
        foreach(var t in positions)
        {
            if(t.name.Contains(a) || t.name.Contains(b))
            {
                properWireColliders.Add(t.GetComponent<Collider>());
            }
        }
    }

    private PossiblePositionsSO GetRandom()
    {
        int rand = Random.Range(0, datas.Count);
        PossiblePositionsSO selectedRandom = datas[rand];
        return selectedRandom == currentData ? GetRandom() : selectedRandom;
    }

}
