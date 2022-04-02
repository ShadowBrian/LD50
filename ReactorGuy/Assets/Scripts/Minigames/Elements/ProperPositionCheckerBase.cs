using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class ProperPositionCheckerBase : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> datas;
        protected ScriptableObject currentData;

        public abstract void CheckPosition();

        protected ScriptableObject GetRandom()
        {
            int rand = Random.Range(0, datas.Count);
            ScriptableObject selectedRandom = datas[rand];
            return selectedRandom == currentData ? GetRandom() : selectedRandom;
        }

    }
}