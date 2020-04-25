using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Player playerScript;

    public float runningSpeed = 0.05f;
    public float rotationSpeed = 0.3f;
    public float distanceForAlert = 10.0f;
    public float startAttackDistance = 5.0f;
    public float damage = 10.0f;
    public float health = 50f;

    public AudioSource[] EnemySounds;
    public AudioSource[] AttackSounds;

    private Animator anim;
    private BoxCollider[] childBoxColliders;

    private NavMeshAgent navMeshAgent;
    private Vector3 positionToGo;
    private bool hasArrived = true;

    private bool isSetDestination = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        childBoxColliders = GetComponentsInChildren<BoxCollider>();

        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(player.position, this.transform.position) < distanceForAlert)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            anim.SetBool("isIdle", false);

            if (direction.magnitude > startAttackDistance)
            {
                navMeshAgent.enabled = true;
                SetDestination();
            }
            else
            {
                hasArrived = true;

                navMeshAgent.enabled = false;
                anim.SetBool("isAttacking", true);
                anim.SetBool("isRunning", false);

                //Give damage to player
                playerScript.TakeDamage(damage * Time.deltaTime);

                playerScript.SetRedScreen();

                System.Random rnd = new System.Random();
                int x = rnd.Next(3);

                bool isAttackPlaying = false;

                for (int i = 0; i < AttackSounds.Length; i++) {
                    if (AttackSounds[i].isPlaying) {
                        isAttackPlaying = true;
                    }
                }
                Debug.Log("AttackSounds.Length ->" + AttackSounds.Length);
                Debug.Log("x ->" + x);

                if (!isAttackPlaying) {
                    AttackSounds[x].Play();
                }
            }
        }
        else {
            if (hasArrived)
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isRunning", false);
                anim.SetBool("isAttacking", false);
            }
            else {
                anim.SetBool("isIdle", false);
                anim.SetBool("isRunning", true);
                anim.SetBool("isAttacking", false);

                if (!EnemySounds[0].isPlaying)
                {
                    EnemySounds[0].Play();
                }
            }
            
        }

        //If enemy arrives at desired destination
        if (transform.position.x == positionToGo.x && transform.position.z == positionToGo.z) {
            anim.SetBool("isIdle", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);
            Debug.Log("Enemy arrived!");
            hasArrived = true;
            isSetDestination = false;
        }

    }

    public void ReduceHealth(float amount, string shotPlace)
    {
        SetDestination();
        health -= amount;
        if (health <= 0)
        {
            Die(shotPlace);
        }
    }

    public void Die(string shotPlace)
    {

        foreach (var b in childBoxColliders) {
            b.isTrigger = true;
        }

        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);

        Destroy(this);
        navMeshAgent.enabled = false;

        if (shotPlace == "ZombieHead")
        {
            anim.SetBool("isHeadshot", true);
        }
        else {
            anim.SetBool("isDead", true);
        }
        
    }

    private void SetDestination() {
        positionToGo = player.position;
        try
        {
            navMeshAgent.SetDestination(positionToGo);
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
        
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", true);
        anim.SetBool("isIdle", false);

        hasArrived = false;

        if (!EnemySounds[0].isPlaying) {
            EnemySounds[0].Play();
        }

        if (!isSetDestination) {
            EnemySounds[1].Play();
            isSetDestination = true;
        }
        
    }
}
