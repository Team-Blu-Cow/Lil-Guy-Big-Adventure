using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class GenericMeleeAttacker : AIBaseBehavior
    {
        public override void run(AICore aiCore) 
        {
            if(enemy_in_attack_range())
            {
                attack();
            }
            else
            {
                move();
            }
        }

        bool enemy_in_attack_range() { return true; } // TODO @matthew - heck if anyone is in range
        void move() { } // TODO @matthew - pathfind towards nearest enemy
        void attack() { } //TODO @matthew - attempt to deal damage to nearest enemy
        
    }
}

