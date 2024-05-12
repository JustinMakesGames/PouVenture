using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum BattleState { Start, PlayerTurn, AttackingTurn, Win, Lose, Flee}
public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    public TMP_Text battleText;
    public GameObject chooseScreen;
    public Transform playerAttack;
    public Transform enemyAttack;

    private BattleState state;
    private PlayerHandler playerHandler;
    private EnemyHandler enemyHandler;
    private Transform player;
    private Transform enemy;

    private Vector3 playerStartPosition;
    private Vector3 enemyStartPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void OnEnable()
    {       
        HandlingStates(BattleState.Start);
    }
    
    public void HandlingStates(BattleState handlingState)
    {
        state = handlingState;
        switch (state)
        {
            case BattleState.Start:
                StartCoroutine(SettingUpBattle());
                StartCoroutine(BattleTextAppears());
                break;
            case BattleState.PlayerTurn:
                PlayerTurn();
                break;
            case BattleState.AttackingTurn:
                StartCoroutine(Attacking());
                break;
            case BattleState.Win:
                StopAllCoroutines();
                WinManagement.instance.WinScreen(playerHandler.exp, playerHandler.maxExp, enemyHandler.exp);
                DestroyingEnemy();
                break;
            case BattleState.Lose:
                //Lose
                break;
            case BattleState.Flee:
                //Flee
                break;
        }
    }
    
    private IEnumerator SettingUpBattle()
    {
        yield return new WaitForEndOfFrame();
        player = FindObjectOfType<BattlePlayerMovement>().transform;
        enemy = FindObjectOfType<EnemyHandler>().transform;
        playerHandler = PlayerHandler.Instance;
        enemyHandler = FindObjectOfType<EnemyHandler>();
        playerStartPosition = player.position;
        enemyStartPosition = enemy.position;
        playerHandler.BattlePlayerSet(player);
        yield return new WaitForSeconds(2);
        player.GetComponent<BattleCamera>().enabled = true;
    }

    private IEnumerator BattleTextAppears()
    {
        yield return new WaitForSeconds(2);
        battleText.text = "A Wild " + enemyHandler.enemyName + " appears!";
        yield return new WaitForSeconds(2);
        HandlingStates(BattleState.PlayerTurn);

    }

    private void PlayerTurn()
    {
        battleText.text = "What will you do?";
        chooseScreen.SetActive(true);
    }

    private IEnumerator Attacking()
    {
        float time = 0f;
        const float attackingLength = 5f;
        player.GetComponent<BattlePlayerMovement>().enabled = true;

        enemyHandler.ChoosingAttack();

        playerAttack.GetComponent<IAttacking>().StartAttack();
        enemyAttack.GetComponent<IAttacking>().StartAttack();
        while (time < attackingLength)
        {
            time += Time.deltaTime;
            if (playerAttack != null)
            {
                playerAttack.GetComponent<IAttacking>().UpdateAttack();
            }

            if (enemy != null)
            {
                enemyAttack.GetComponent<IAttacking>().UpdateAttack();
            }
            
            yield return null;
        }

        StartCoroutine(HandlingEndOfTurn());
    }

    private IEnumerator HandlingEndOfTurn()
    {
        player.GetComponent<BattlePlayerMovement>().enabled = false;
        PuttingObjectsAtPosition();
        RemovingAllProjectiles();
        bool areObjectsReturned = false;
        while (!areObjectsReturned)
        {          
            if (AreObjectsAtStartPosition())
            {
                areObjectsReturned = true;
            }
            yield return null;
        }
        PuttingStartRotation();
        HandlingStates(BattleState.PlayerTurn);
    }
    
    private void PuttingObjectsAtPosition()
    {
        player.GetComponent<NavMeshAgent>().enabled = true;
        player.GetComponent<NavMeshAgent>().SetDestination(playerStartPosition);
        enemy.GetComponent<NavMeshAgent>().SetDestination(enemyStartPosition);
    }

    private void RemovingAllProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    private void PuttingStartRotation()
    {
        player.rotation = Quaternion.identity;
        enemy.rotation = Quaternion.Euler(0, 180, 0);
        player.GetComponent<NavMeshAgent>().enabled = false;
    }

    private bool AreObjectsAtStartPosition()
    {
        float playerDistance = Vector3.Distance(player.position, playerStartPosition);
        float enemyDistance = Vector3.Distance(enemy.position, enemyStartPosition);
        if (playerDistance < 0.02f && enemyDistance < 0.02f)
        {
            return true;
        }

        return false;
    }

    
    private void DestroyingEnemy()
    {
        Destroy(enemy.gameObject);
    }
    
    
    
}
