using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SideLaserBehaviour : EnemyProjectile
{
    public float timer;
    public float endTimer;
    private bool isAttacking;
    private bool isPreparing;
    public float rotationSpeed = 20f;
    private void Start()
    {
        Destroy(gameObject, 5f);
        StartCoroutine(Attacking());
    }

    private void Update()
    {
        if (isAttacking)
        {
            transform.Translate(Vector3.forward * stats.secondAttackSpeed * Time.deltaTime);
        }
    }

    private IEnumerator Attacking()
    {
        while (timer < endTimer)
        {
            timer += Time.deltaTime;
            PrepareAttack();
            yield return null;
        }

        isPreparing = true;
        StartCoroutine(PreparingAttack());
    }

    private IEnumerator PreparingAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = true;
        Destroy(transform.GetChild(0).gameObject);
    }

    private void PrepareAttack()
    {
        Vector3 direction = player.position - transform.position;

        Quaternion lookDirection = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotationSpeed * Time.deltaTime);
    }


}