using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    // different ai can be run using different scripts, they should all inherit from this
    internal abstract class AIBaseBehavior : MonoBehaviour
    {
        public abstract void run(AICore aiCore);
    }

    // a AI core must exist in every scene, this controls all ai within the scene
    // this inclues non combatants, eg birds

    // if a actor is removed from the scene, it must be removed from the ai controller or it will crash.

    public class AICore : MonoBehaviour
    {
        public GameObject player;
        public List<GameObject> ai_list;

        private void Start()
        {
            // removes all null references from the list
            ai_list.Remove(null);
        }

        public bool has_actor(GameObject ai_actor)
        {
            return ai_list.Contains(ai_actor);
        }

        public void add(GameObject ai_actor)
        {
            if (has_actor(ai_actor) && ai_actor != null)
            {
                return;
            }

            ai_list.Add(ai_actor);
        }

        public void remove(GameObject ai_actor)
        {
            ai_list.Remove(ai_actor);
        }

        private void FixedUpdate()
        {
            foreach (GameObject actor in ai_list)
            {
                // this will always cache miss however it allows the behaviour script to be changed out at runtime
                AIBaseBehavior behaviour = actor.GetComponent<AIBaseBehavior>();
                if (behaviour)
                {
                    behaviour.run(this);
                }
            }
        }
    }
}