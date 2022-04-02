using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ProperWirePositionChecker : ProperPositionCheckerBase
    {
        private List<WirePosition> positions = new List<WirePosition>();
        public List<Collider> properWireColliders = new List<Collider>();
        private PossiblePositionsWireSO properCurrentData;

        private void Awake()
        {
            foreach(Transform t in transform)
            {
                positions.Add(t.GetComponent<WirePosition>());
            }
        }

        public override void CheckPosition()
        {
            currentData = GetRandom();
            properCurrentData = (PossiblePositionsWireSO)currentData;
            string chosen = null;
            (string a, string b) = properCurrentData.GetPositions();
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

    }
}