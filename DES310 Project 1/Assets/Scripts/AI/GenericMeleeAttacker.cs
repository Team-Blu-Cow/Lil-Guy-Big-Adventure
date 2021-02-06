using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class GenericMeleeAttacker : AIBaseBehavior
    {
        public override void run(AICore aiCore) 
        {
            GameObject closest = find_closest_enemy();

            if(enemy_in_attack_range(closest))
            {
                attack(closest);
            }
            else
            {
                move(closest);
            }
        }

        GameObject find_closest_enemy() { return null; } // TODO @matthew - calculate distance to targets
        bool enemy_in_attack_range(GameObject enemy) { return true; } // TODO @matthew - check if target
        void move(GameObject enemy) { } // TODO @matthew - pathfind towards nearest enemy
        void attack(GameObject enemy) { } //TODO @matthew - attempt to deal damage to nearest enemy
        
    }
}

