using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WiresPositions", fileName = "WiresPositions")]
public class PossiblePositionsWireSO : ScriptableObject
{
    [SerializeField] bool _0_0 = false;
    [SerializeField] bool _0_1 = false;
    [SerializeField] bool _0_2 = false;
    [SerializeField] bool _0_3 = false;
    [SerializeField] bool _1_0 = false;
    [SerializeField] bool _1_1 = false;
    [SerializeField] bool _1_2 = false;
    [SerializeField] bool _1_3 = false;
    [SerializeField] bool _2_0 = false;
    [SerializeField] bool _2_1 = false;
    [SerializeField] bool _2_2 = false;
    [SerializeField] bool _2_3 = false;
    [SerializeField] bool _3_0 = false;
    [SerializeField] bool _3_1 = false;
    [SerializeField] bool _3_2 = false;
    [SerializeField] bool _3_3 = false;

    public (string, string) GetPositions()
    {
        //XD
        List<(string name, bool isOn)> boolsTemp = new List<(string name, bool isOn)>();
        boolsTemp.Add(("_0_0", _0_0));
        boolsTemp.Add(("_0_1", _0_1));
        boolsTemp.Add(("_0_2", _0_2));
        boolsTemp.Add(("_0_3", _0_3));
        boolsTemp.Add(("_1_0", _1_0));
        boolsTemp.Add(("_1_1", _1_1));
        boolsTemp.Add(("_1_2", _1_2));
        boolsTemp.Add(("_1_3", _1_3));
        boolsTemp.Add(("_2_0", _2_0));
        boolsTemp.Add(("_2_1", _2_1));
        boolsTemp.Add(("_2_2", _2_2));
        boolsTemp.Add(("_2_3", _2_3));
        boolsTemp.Add(("_3_0", _3_0));
        boolsTemp.Add(("_3_1", _3_1));
        boolsTemp.Add(("_3_2", _3_2));
        boolsTemp.Add(("_3_3", _3_3));

        (string a, string b) positions = ("_0_0", "_3_3");

        foreach(var item in boolsTemp)
        {
            if(item.isOn)
            {
                positions.a = item.name;
                boolsTemp.Remove(item);
                break;
            }
        }

        foreach(var item in boolsTemp)
        {
            if(item.isOn)
            {
                positions.b = item.name;
                break;
            }
        }

        return positions;
    }
}
