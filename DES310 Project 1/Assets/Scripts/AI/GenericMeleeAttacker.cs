using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AI
{
    public class GenericMeleeAttacker : AIBaseBehavior
    {
        public float speed = 1f;
        // public int attack_range = 1;

        public override void Move(AICore aiCore, int distance = 1)
        {
            turn_completed = false;
            Vector3[] path = FindShortestPathToEnemy(aiCore);
           
            if(path != null)
            {
                StartCoroutine(MoveCoroutine(path, distance));
            }
            else
            {
                turn_completed = true;
            }
        }
        public override AbilityResult Attack(AICore aiCore)
        {
            turn_completed = false;
            AbilityResult result = new AbilityResult();
            // TODO @matthew
            // check target is in range
            // deal damage

            Vector3[] path = FindShortestPathToEnemy(aiCore);
            if(path != null)
            {
                if (IsInAttackRange(path))
                {
                    if(GetAbility(0) != null)
                    {
                        Vector3 targetLoc = path[path.Length - 1];
                        IsoNode node = aiCore.pathfinder.GetGrid().WorldToNode(targetLoc);

                        if(node != null && node.occupier != null)
                        {
                            GameObject target = node.occupier;
                            GetComponent<TestCombatSystem>().enemy = target;
                            result = GetComponent<Combatant>().UseAbility(0);
                            result.target = target;
                            result.abilityIndex = 0;
                        }
                    }
                }
            }
            
            turn_completed = true;
            return result;
        }

        Vector3[] FindShortestPathToEnemy(AICore aiCore)
        {
            List<Vector3[]> path_list = new List<Vector3[]>();

            Vector3 position = GetComponent<Transform>().position;

            // find paths
            foreach (GameObject actor in aiCore.party_list)
            {
                Vector3 actor_pos = actor.GetComponent<Transform>().position;

                // set target node to unoccupied
                IsoNode node = aiCore.pathfinder.GetGrid().WorldToNode(actor_pos);
                node.occupied = false;


                Vector3[] path = aiCore.pathfinder.FindPath(position, actor_pos);
                if (path != null)
                {
                    path_list.Add(path);
                }

                node.occupied = true;
            }

            Vector3[] shortest = null;
            foreach (Vector3[] path in path_list)
            {
                if (shortest == null)
                {
                    shortest = path;
                    continue;
                }

                if (path.Length < shortest.Length)
                {
                    shortest = path;
                }
            }

            return shortest;
        }

        bool IsInAttackRange(Vector3[] path)
        {
            if (path.Length > AbilityAttackRange(0))
                return false;
            else
                return true;
        }

        // moves the ai along the path
        private IEnumerator MoveCoroutine(Vector3[] path, int movement_distance)
        {
            int targetIndex = 0;
            Vector3 currentWaypoint = path[targetIndex];

            while (true)
            {
                if (path.Length == 1)
                {
                    break;
                }

                if (path.Length - targetIndex <= AbilityAttackRange(0))
                {
                    break;
                }



                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        break;
                    }
                    if (targetIndex == movement_distance)
                    {
                        break;
                    }
                    if (path.Length - targetIndex <= AbilityAttackRange(0))
                    {
                        break;
                    }


                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                yield return null;
            }
            turn_completed = true;
        }

        // TEMP - to be removed when a proper AI is written
        Ability GetAbility(int index)
        {
            Ability[] abilities = GetComponent<Combatant>().abilitiesUsing;
            if (abilities != null)
            {
                if(abilities.Length > index)
                {
                    if (abilities[index] != null)
                    {
                        return abilities[index];
                    }
                }
            }

            return null;
        }

        // TEMP - to be removed when a proper AI is written
        int AbilityAttackRange(int index)
        {

            Ability ability = GetAbility(index);
            
            if(ability != null)
                return ability.abilityRange;
            else
                return 0;
        }



    }
}


