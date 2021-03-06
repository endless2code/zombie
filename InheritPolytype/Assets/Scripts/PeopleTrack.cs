﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;      //引用 系統.查詢語言 Lin Query

public class PeopleTrack : People
{
    /// <summary>
    /// 目標
    /// </summary>
    protected Transform target;

    /// <summary>
    /// 人類陣列
    /// </summary>
    public People[] people;

    public float[] distance;

    protected virtual void Start()
    {
        //人類陣列 = 透過類型尋找物件<泛型>()
        people=FindObjectsOfType<People>();
        distance = new float[people.Length];
    }

    private void Update()
    {
        Track();
    }

    /// <summary>
    /// 追蹤方法
    /// </summary>
    protected virtual void Track()
    {
        //儲存所有人跟此物件的距離
        for (int i = 0; i < people.Length; i++)
        {
            if (people[i]==null || people[i].transform.name == "殭屍" || people[i].transform.name == "警察")
            {
                if (people[i]==null)                //如果人類死亡 距離改為1000
                {
                    distance[i] = 1000;         
                }
                else
                {
                    distance[i] = 999;              //與殭屍物件的距離改為999
                 }
                continue;                           //繼續 - 跳過並執行下一次迴圈
            }
        distance[i] =  Vector3.Distance(transform.position, people[i].transform.position);
        }
        //判斷距離
        //print("最近的距離 : " + distance.Min());
        float min = distance.Min();                                 //最小值
        int index = distance.ToList().IndexOf(min);        //索引值 = 距離.轉清單.取得索引值(最小值)
        target = people[index].transform;                       //目標 = 人類[索引值].變形
        //追蹤最近目標
        agent.SetDestination(target.position);
        if (agent.remainingDistance <=1f && min != 999)
        {
            HitPeople();
        }
    }

    private float timer;
    private void HitPeople()
    {

        if (timer>=1f)
        {
            timer = 0;
            agent.isStopped = true;
            ani.SetTrigger("攻擊");
            target.GetComponent<People>().Dead();
        }
        else
        {
            agent.isStopped = false;
            timer += Time.deltaTime;
        }

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "火球")
        {
            Dead();
        }
    }


}
