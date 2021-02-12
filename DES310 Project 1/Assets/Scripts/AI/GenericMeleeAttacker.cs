using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace AI
{
    public class GenericMeleeAttacker : AIBaseBehavior
    {

        PathFinder pathFinder;
        Mutex mutex = new Mutex();
        int paths_finished = 0;
        int path_count = 0;
        List<Vector3[]> path_list = new List<Vector3[]>();
        AICore ai_core;

        public override void run(AICore aiCore) 
        {
            ai_core = aiCore;
            pathFinder = aiCore.pathfinder;
            paths_finished = 0;
            path_count = 0;


            BeginPathfindingCoroutines(aiCore);

        }

        void OnPathFound(Vector3[] newPath, bool pathSuccess)
        {
            mutex.WaitOne();
            
            if (pathSuccess)
            {
                path_list.Add(newPath);
                Debug.Log("path found");
            }
            else
            {
                Debug.Log("path failed");
            }
            paths_finished++;


            if(paths_finished == path_count)
            {
                Vector3[] closest_path = FindClosestPartyMemberPath(path_list);

                if (IsEnemyInAttackRange(closest_path))
                {
                    GameObject enemy = GetPartyMemberObjectFromPath(closest_path, ai_core);
                    attack(enemy);
                }
                else
                {
                    move(closest_path);
                }
            }


            mutex.ReleaseMutex();
        }

        Vector3[] FindClosestPartyMemberPath(List<Vector3[]> list) 
        {
            Vector3[] closest = null;
            
            foreach(Vector3[] path in list)
            {
                if(closest == null)
                {
                    closest = path;
                    continue;
                }

                if(path.Length < closest.Length)
                {
                    closest = path;
                }
            }

            return closest;
        }

        GameObject GetPartyMemberObjectFromPath(Vector3[] path, AICore aiCore)
        {
            if (path == null)
                return null;

            Vector3 pos = path[path.Length - 1];
            foreach(GameObject entity in aiCore.party_list)
            {
                if(entity.GetComponent<Transform>().position == pos)
                {
                    return entity;
                }
            }
            return null;
        }

        bool IsEnemyInAttackRange(Vector3[] path) 
        {
            if (path == null)
                return false;

            if (path.Length > 1)
                return false;
            else
                return true;        
        }

        void BeginPathfindingCoroutines(AICore aiCore)
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

        void move(Vector3[] path)
        {
            GetComponent<Transform>().position = path[0];
        }

        void attack(GameObject enemy) 
        {
            enemy.GetComponent<Combatant>().do_damage(1, Aspects.Aspect.None);
        } 

    }
}





