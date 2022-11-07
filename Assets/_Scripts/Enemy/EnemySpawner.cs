using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabEnemies;      // массив префабов с зомби
    NavMeshAgent agent;

    public bool active;
    public float cooldown = 1f;             // перезарядка спауна
    private float lastSpawn;


    void Update()
    {
        if (active && Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;
            // тут добавить эффект появления
            Invoke("SpawnEnemy", 1);
            

            /*            float dist = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
                        if (dist > radius)
                        {
                            SpawnEnemy();
                        }*/
        }
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);            // выбираем рандом из массива врагов
        GameObject go = Instantiate(prefabEnemies[ndx]);            // создаём префаб        

        go.transform.SetParent(transform, false);                   // Назначаем этот спавнер родителем
        agent = go.GetComponent<NavMeshAgent>();                    // Находим НавМешАгент
        agent.Warp(transform.position);                             // Перемещаем префаб к спавнеру
    }
}
