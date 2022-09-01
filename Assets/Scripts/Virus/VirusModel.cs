using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VirusModel
{
    public enum InfectionState{Immune, Healthy, Infected }
    public enum MovingState { Wandering, Chasing }

    [Serializable]
    public class VirusMaterials
    {
        public Material material;        
    }
}
