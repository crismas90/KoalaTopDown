using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("������� ������")]
    public GameObject[] prefabEnemies;      // ������ �������� � �����

    public bool active;                     // ������� ��� ���
    public bool chasePlayer;                // ������� ������ ����������� �����
    public float chaseDistance;             // ��������� �������� ������
    public float cooldown = 1f;             // ����������� ������    
    //public int enemyTriggerDistance;        // ���������� ��������� ������� ������
    private float lastSpawn;


    void Update()
    {
        if (active && Time.time - lastSpawn > cooldown)
        {
            lastSpawn = Time.time;
            // ��� �������� ������ ���������
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
        int ndx = Random.Range(0, prefabEnemies.Length);            // �������� ������ �� ������� ������
        GameObject go = Instantiate(prefabEnemies[ndx]);            // ������ ������
        go.transform.SetParent(transform, false);                   // ��������� ���� ������� ���������
        agent = go.GetComponent<NavMeshAgent>();                    // ������� �����������
        agent.Warp(transform.position);                             // ���������� ������ � ��������
        if(chasePlayer)
            go.GetComponent<BotAI>().triggerLenght = chaseDistance;                // ������������� ������������� �� �������
    }

    public void ActivateSpawner()
    {
        active = !active;
    }
}
