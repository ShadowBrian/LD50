using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProperFixChecker : ProperPositionCheckerBase
    {
        [SerializeField] private List<GameObject> wires = new List<GameObject>();
        private MinigameBase minigameBase;
        private FixPossiblesSO properCurrentData;
        private string firstTool;
        private string secondTool;
        private string thirdTool;
        private int currentStep;

        [SerializeField] private List<GameObject> lamps = new List<GameObject>();
        private List<Renderer> renderers = new List<Renderer>();
        private List<MaterialPropertyBlock> propertyBlocks = new List<MaterialPropertyBlock>();

        private void Awake()
        {
            minigameBase = GetComponentInParent<MinigameBase>(); //XD

            foreach(var lamp in lamps)
            {
                Renderer lampRenderer = lamp.GetComponent<Renderer>();
                renderers.Add(lampRenderer);
                MaterialPropertyBlock newPB = new MaterialPropertyBlock();
                lampRenderer.GetPropertyBlock(newPB);
                propertyBlocks.Add(newPB);
            }
        }

        public override void CheckPosition()
        {
            currentData = GetRandom();
            properCurrentData = (FixPossiblesSO)currentData;

            (string firstTool, string secondTool, string thirdTool, string wireNumber) = properCurrentData.GetInfo();
            this.firstTool = firstTool;
            this.secondTool = secondTool;
            this.thirdTool = thirdTool;
            foreach(var wire in wires)
            {
                wire.SetActive(false);
                if(wire.name.Contains(wireNumber))
                {
                    wire.SetActive(true);
                }
            }

            currentStep = 0;
            LightLamps();
        }

        public void CheckTool(string toolName)
        {
            if(currentStep == 0)
            {
                if(toolName.Contains(firstTool))
                {
                    currentStep++;
                }
                else
                {
                    currentStep = 0;
                }
            }
            else if(currentStep == 1)
            {
                if(toolName.Contains(secondTool))
                {
                    currentStep++;
                }
                else
                {
                    currentStep = 0;
                }
            }
            else if(currentStep == 2)
            {
                if(toolName.Contains(thirdTool))
                {
                    currentStep++;
                }
                else
                {
                    currentStep = 0;
                }
            }
            LightLamps();

            if(currentStep >= 3)
            {
                Debug.Log("Game finished override");
                minigameBase.EndMinigame();
            }
        }

        private void LightLamps()
        {
            for(int i = 0; i < currentStep; i++)
            {
                propertyBlocks[i].SetColor("_BaseColor", Color.green);
                renderers[i].SetPropertyBlock(propertyBlocks[i]);
            }
            for(int i = currentStep; i < lamps.Count; i++)
            {
                propertyBlocks[i].SetColor("_BaseColor", Color.red);
                renderers[i].SetPropertyBlock(propertyBlocks[i]);
            }
        }
    }
}