using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProperSliderPositionChecker : ProperPositionCheckerBase
    {
        private List<ClickablePosition> clickablePositions = new List<ClickablePosition>();
        private List<SliderPosition> sliderPositions = new List<SliderPosition>();
        private PossiblePositionsSliderSO properCurrentData;

        private void Awake()
        {
            foreach(Transform t in transform)
            {
                if(t.TryGetComponent(out ClickablePosition clickablePosition))
                    clickablePositions.Add(clickablePosition);
                else if(t.TryGetComponent(out SliderPosition sliderPosition))
                    sliderPositions.Add(sliderPosition);
            }
        }


        public override void CheckPosition()
        {
            currentData = GetRandom();
            properCurrentData = (PossiblePositionsSliderSO)currentData;

            List<(string name, bool isOn)> clickables = properCurrentData.GetClickables();

            foreach(var position in clickablePositions)
            {
                foreach(var clickable in clickables)
                {
                    if(position.name.Contains(clickable.name))
                    {
                        position.SetClickable(clickable.isOn);
                        break;
                    }
                }
            }

            List<(string name, float value)> sliders = properCurrentData.GetSliders();

            foreach(var position in sliderPositions)
            {
                foreach(var slider in sliders)
                {
                    if(position.name.Contains(slider.name))
                    {
                        position.SetSlider(slider.value);
                        break;
                    }
                }
            }
        }

    }
}