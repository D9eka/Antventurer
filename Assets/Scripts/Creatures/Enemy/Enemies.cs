using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Enemy
{
    public static class Enemies
    {
        private static List<Transform> enemiesList = new();

        public static void AddEnemy(Transform enemy)
        {
            enemiesList.Add(enemy);
            Debug.Log("Add");
        }

        public static void RemoveEnemy(Transform enemy) 
        { 
            if(enemiesList.Contains(enemy)) 
            { 
                enemiesList.Remove(enemy);
            }
        }

        public static bool TryFindNearestEnemy(Transform enemy, float maxDistance, out Transform nearestEnemy)
        {
            nearestEnemy = null;
            float minimalDistance = Mathf.Infinity;
            foreach(Transform enemyTransform in enemiesList) 
            { 
                if(enemyTransform == enemy)
                    continue;
                float distance = Vector2.Distance(enemy.position, enemyTransform.position);
                if(Mathf.Abs(enemy.position.x - enemyTransform.position.x) < 1f && distance < minimalDistance)
                {
                    minimalDistance = distance;
                    nearestEnemy = enemyTransform;
                }
            }
            return nearestEnemy != null && minimalDistance < maxDistance;
        }

        public static void CleanEnemiesList()
        {
            enemiesList.Clear();
            Debug.Log("Clean");
        }
    }
}