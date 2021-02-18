using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// TODO @matthew - can attack after moving

namespace AI
{
    public class GenericMeleeAttacker : AIBaseBehavior
    {
        public int movement_distance = 1;
        public float speed = 1f;

        private PathFinder pathFinder;
        private Mutex mutex = new Mutex(); // coroutines are still run single threaded, this may be overkill
        private int paths_finished = 0;
        private int path_count = 0;
        private List<Vector3[]> path_list = new List<Vector3[]>();
        private AICore ai_core;

        public int attack_range = 1;

        // entry point, starts up coroutines and sets the turn_completed flag
        public override void run(AICore aiCore)
        {
            ai_core = aiCore;
            pathFinder = aiCore.pathfinder;
            paths_finished = 0;
            path_count = 0;
            turn_completed = false;
            BeginPathfindingCoroutines(aiCore);
        }

        // run by the pathfinding system when a path is found or its determined impossible
        private void OnPathFound(Vector3[] newPath, bool pathSuccess)
        {
            mutex.WaitOne();// blocks until all paths have been added to the queue

            if (pathSuccess)
            {
                path_list.Add(newPath);
            }
            paths_finished++;

            // only run after the final path has been found
            if (paths_finished == path_count)
            {
                Vector3[] closest_path = FindClosestPartyMemberPath(path_list);

                if (IsEnemyInAttackRange(closest_path))
                {
                    // target is in ajacent tile
                    GameObject enemy = GetPartyMemberObjectFromPath(closest_path, ai_core);
                    attack(enemy);
                }
                else
                {
                    // move towards enemy
                    StartCoroutine(move(closest_path));
                    path_list.Clear();
                }
            }

            mutex.ReleaseMutex();
            turn_completed = true;
        }

        private Vector3[] FindClosestPartyMemberPath(List<Vector3[]> list)
        {
            Vector3[] closest = null;

            foreach (Vector3[] path in list)
            {
                if (closest == null)
                {
                    closest = path;
                    continue;
                }

                if (path.Length < closest.Length)
                {
                    closest = path;
                }
            }

            return closest;
        }

        // get a reference to the combatant at the end of the path
        // this could be done by storing the object when requesting the path but that requires a refactor of other system
        // this will be good enough
        private GameObject GetPartyMemberObjectFromPath(Vector3[] path, AICore aiCore)
        {
            if (path == null)
                return null;

            if (path.Length == 0)
                return null;

            Vector3 pos = path[path.Length - 1];
            foreach (GameObject entity in aiCore.party_list)
            {
                if (entity.GetComponent<Transform>().position == pos)
                {
                    return entity;
                }
            }
            return null;
        }

        // checks if a enemy is in a attack range of the combatants current location
        private bool IsEnemyInAttackRange(Vector3[] path)
        {
            if (path == null)
                return false;

            if (path.Length > attack_range)
                return false;
            else
                return true;
        }

        // checks if an enemy is in attack rage on a given tile on the path
        private bool IsEnemyInAttackRange(Vector3[] path, int next_position)
        {
            if (path == null)
                return false;

            if (path.Length - next_position > attack_range)
                return false;
            else
                return true;
        }

        // sets up the coroutines that calculate the path
        private void BeginPathfindingCoroutines(AICore aiCore)
        {
            if (aiCore == null)
                return;

            if (aiCore.party_list.Count == 0)
                return;

            Vector3 start_pos = GetComponent<Transform>().position;
            path_count = 0;
            if (pathFinder)
            {
                mutex.WaitOne();
                foreach (GameObject actor in aiCore.party_list)
                {
                    Vector3 end_pos = actor.GetComponent<Transform>().position;
                    PathRequestManager.RequestPath(start_pos, end_pos, OnPathFound);
                    path_count++;
                }
                mutex.ReleaseMutex();
            }
        }

        // moves the ai along the path
        private IEnumerator move(Vector3[] path)
        {
            int targetIndex = 0;
            Vector3 currentWaypoint = path[targetIndex];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    if (targetIndex == movement_distance)
                    {
                        yield break;
                    }
                    if (IsEnemyInAttackRange(path, targetIndex))
                    {
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                yield return null;
            }
        }

        // deal damage to the enemy
        // TODO @matthew - this should use the combatants abilities insted of just dealing damage
        private void attack(GameObject enemy)
        {
            if(enemy)
            {
                enemy.GetComponent<Combatant>().do_damage(1, Aspects.Aspect.None);
            }
            else
            {
                Debug.Log("AI: enemy reference was null");
            }
        } 
    }
}