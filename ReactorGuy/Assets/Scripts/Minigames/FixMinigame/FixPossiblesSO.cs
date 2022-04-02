using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FixPossibles", fileName = "FixPossibles")]
public class FixPossiblesSO : ScriptableObject
{
    [SerializeField] private string firstTool = "0";
    [SerializeField] private string secondTool = "0";
    [SerializeField] private string thirdTool = "0";
    [SerializeField] private string wireNumber = "0";

    public (string firstTool, string secondTool, string thirdTool, string wireNumber) GetInfo()
    {
        return (firstTool, secondTool, thirdTool, wireNumber);
    }
}
