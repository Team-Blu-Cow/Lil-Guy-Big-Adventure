using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace AI
{
    // different ai can be run using different scripts, they should all inherit from this
    public abstract class AIBaseBehavior : MonoBehaviour
    {
        //public abstract void run(AICore aiCore);

        public abstract void Move(AICore aiCore, int distance);
        public abstract AbilityResult Attack(AICore aiCore);


        // signifies if a combatant has completed all animation
        [ReadOnly]
        public bool turn_completed = true;
    }
}
