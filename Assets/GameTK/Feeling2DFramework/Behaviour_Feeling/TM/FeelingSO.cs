using System;
using UnityEngine;

namespace NJM.Template {

    [CreateAssetMenu(fileName = "So_Feeling_", menuName = "NJM/Feeling", order = 1)]
    public class FeelingSO : ScriptableObject {
        public FeelingTM tm;
    }
}