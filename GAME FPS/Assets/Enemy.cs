using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : TargetScript
{
    public Animator animator;
    public float health = 100;
    public NavMeshAgent navMeshAgent;
    bool isDead;

    float coolDown = 0.5f;
    Transform target;


    public GameObject deadEffect;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance < 1.5 && !isDead) //---Khi quái vật tiếp cận một khoảng cách nhỏ hơn 1.5 đơn vị thì sẽ tấn công bạn
        {
            animator.SetTrigger("Attack");
        }

        if(isHit && coolDown <= 0 && !isDead)
        {
            Debug.Log("Hit");
            
            health -= 20;   //---Khi trúng đạn thì máu của quái vật sẽ trừ đi 20 trên tổng số 100 máu tức là sau 5 lần bắn trúng thì quái vật sẽ chết
            coolDown = 0.5f;

            if(health <= 0)  //---Khi máu của quái vật là 0 thì chúng sẽ chết
            {
                animator.SetTrigger("Dead");
                navMeshAgent.isStopped = true;
                isDead = true;
                StartCoroutine(Dead());
            }
            else
            {
                animator.SetTrigger("Hurt");  //--Khi bị bắn trúng thì quái vật sẽ bị khựng lại
                navMeshAgent.isStopped = true;
            }

            isHit = false;
        }

        else if(coolDown <= 0)
        {
            if(!isDead)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.position);
            }
        }

        if(coolDown > 0)
        {
            coolDown -= Time.deltaTime; 
        }

        //---Co the loi

    }

    IEnumerator Dead() //---Khi chết tạo ra hiệu ứng nổ
    {
        yield return new WaitForSeconds(1.5f);
        GameObject _effect = Instantiate(deadEffect, transform.position, Quaternion.identity);
        Destroy(_effect, 3f);
        Destroy(gameObject);
    }
}
