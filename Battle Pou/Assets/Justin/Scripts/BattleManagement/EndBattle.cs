using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBattle : MonoBehaviour
{
    public static EndBattle instance;

    public Transform arenaSpawn;
    public GameObject battleArena;
    private GameObject battleArenaClone;
    private Transform enemySpawn;
    private Transform cam;

    public GameObject overworldEnemy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SettingUpDeletion()
    {
        cam = Camera.main.transform;
        battleArenaClone = FindObjectOfType<BattleManager>().gameObject;
    }

    public void GetEnemy(GameObject enemy)
    {
        overworldEnemy = enemy;
    }
    public void DestroyEnemy()
    {
        Destroy(overworldEnemy);
    }

    private IEnumerator DestroyBattleField()
    {
        yield return new WaitUntil(() => Camera.main.fieldOfView <= 30);
        Destroy(battleArenaClone);
    }

    public void StartingCoroutines()
    {
        StartCoroutine(SetCameraAtPosition());
        StartCoroutine(EnableOverworldEnemies());
        StartCoroutine(DestroyBattleField());
    }

    private IEnumerator SetCameraAtPosition()
    {
        yield return new WaitUntil(() => Camera.main.fieldOfView <= 30);
        cam.position = CreateBattleArena.instance.oldCamPosition;
        cam.rotation = CreateBattleArena.instance.oldCamRotation;
    }

    private IEnumerator EnableOverworldEnemies()
    {
        yield return new WaitUntil(() => Camera.main.fieldOfView <= 30);
        CreateBattleArena.instance.OverworldManagement();
    }

    public void KeepEnemyAlife()
    {
        StartCoroutine(KeepImmuneFrames());
    }

    private IEnumerator KeepImmuneFrames()
    {
        yield return new WaitUntil(() => Camera.main.fieldOfView <= 30);
        overworldEnemy.GetComponent<EnemyOverworld>().enabled = false;
        MeshRenderer renderer = overworldEnemy.GetComponent<MeshRenderer>();
        int amountOfFrames = 20;

        for (int i = 0; i < amountOfFrames; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        overworldEnemy.GetComponent<EnemyOverworld>().enabled = true;
    }
}