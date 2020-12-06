using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class RunModifierManager : MonoBehaviour
    {
        // generate new run parameters
        // activate the modifiers when you reach the room
        RunModifiers firstMod, secondMod;
        int firstRoom, secondRoom;

        public void GE_GenerateNewRun()
        {
            System.Random rnd = new System.Random();
            firstMod = (RunModifiers)rnd.Next();
            firstRoom = rnd.Next(1, 4); //equal 25% chance
        }
    }

    [System.Serializable]
    public class RunParameters
    {
        internal RunModifiers firstMod, secondMod; 
        internal int firstRoom, secondRoom;

        // run params constructor
        public RunParameters(RunModifiers _firstMod, int _firstRoom, RunModifiers _secondMod = default, int _secondRoom = default)
        {
            firstMod = _firstMod;
            firstRoom = _firstRoom;
            secondMod = _secondMod;
            secondRoom = _secondRoom;
        }
    }
}