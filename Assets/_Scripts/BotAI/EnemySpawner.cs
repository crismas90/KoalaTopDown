using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Префабы врагов")]
    public GameObject[] prefabEnemies;      // массив префабов с зомби

    public bool active;                     // активен или нет
    public bool chasePlayer;                // триггер врагов срабатывает сразу
    public float chaseDistance;             // дистанция триггера врагов
    public float cooldown = 1f;             // перезарядка спауна    
    //public int enemyTriggerDistance;        // установить дистанцию тригера врагов
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
        go.transform.SetParent(transform, false);                   // назначаем этот спавнер родителем
        agent = go.GetComponent<NavMeshAgent>();                    // находим НавМешАгент
        agent.Warp(transform.position);                             // перемещаем префаб к спавнеру
        if(chasePlayer)
            go.GetComponent<BotAI>().triggerLenght = chaseDistance;                // устанавливаем преследование за игроком
    }

    public void ActivateSpawner()
    {
        active = !active;
    }
}
