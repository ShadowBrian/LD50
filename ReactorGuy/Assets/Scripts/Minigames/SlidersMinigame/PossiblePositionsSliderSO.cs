using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SliderPositions", fileName = "SliderPositions")]
public class PossiblePositionsSliderSO : ScriptableObject
{
    [SerializeField] bool _0_0 = false;
    [SerializeField] bool _0_1 = false;
    [SerializeField] bool _1_0 = false;
    [SerializeField] bool _1_1 = false;

    [SerializeField] float _a = 0;
    [SerializeField] float _b = 0;
    [SerializeField] float _c = 0;


    public List<(string name, bool isOn)> GetClickables()
    {
        //XD
        List<(string name, bool isOn)> tempList = new List<(string name, bool isOn)>();

        tempList.Add(("_0_0", _0_0));
        tempList.Add(("_0_1", _0_1));
        tempList.Add(("_1_0", _1_0));
        tempList.Add(("_1_1", _1_1));

        return tempList;
    }

    public List<(string name, float value)> GetSliders()
    {
        //XD
        List<(string name, float value)> tempList = new List<(string name, float value)>();

        tempList.Add(("_a", _a));
        tempList.Add(("_b", _b));
        tempList.Add(("_c", _c));

        return tempList;
    }


}
