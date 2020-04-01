using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStone : MonoBehaviour, IMoveDown
{

    public float delay;
    public int NumberOfScene { get; set; }

    public void MoveObjectDown(float delayTime)
    {
        delay = delayTime;
        StartCoroutine(Down());
    }


    private IEnumerator Down()
    {


        yield return new WaitForSeconds(delay);

        //Debug.Log("Я блок! Пытаюсь двигаться!");


        Vector2 pos = transform.position;
        Vector2 direction = Vector2.up;
        float distance = 0.6f;
        //стреляем вверх и запоиманем в hit_up что сверху
        RaycastHit2D hit_up = Physics2D.Raycast(pos, direction, distance, 1 << 9);

        bool trigger;
        do
        {
            trigger = false; //изначально проверка не нужна
            pos = transform.position;
            direction = Vector2.down;
            distance = 0.6f;
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, distance, 1 << 10); //стреляю вниз
            if (hit.collider == null) //если внизу свободно, то двигаюсь вниз
            {
                //Debug.Log("Я блок! Двигаюсь вниз! Свободно!");
                transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
                trigger = true;  //нужная еще одна проверка
            }
            else
            {
                trigger = false;
                //Debug.Log("Я блок! Не могу падать, подомной стоит " + hit.collider.tag);
                //if (hit.collider.tag == "stairs")
                //    if (hit.collider.GetComponent<stairsController>())
                //    {
                //        Destroy(hit.collider.gameObject); //если лестница, то уничтожаем её
                //        transform.position = new Vector2(transform.position.x, transform.position.y - 1f);  //вдигаемся вниз
                //        trigger = true;  // нужна еще проверка
                //    }
            }
        } while (trigger);


        //если сверху была не земля, то двигаем то что сверху вниз
        if (hit_up.collider != null)
            //if (hit_up.collider.tag != "blockGround")
            if (hit_up.collider.GetComponent<BlockGroundDefault>())

            {
                IMoveDown gm = hit_up.collider.GetComponent<IMoveDown>();
                gm.MoveObjectDown(0);
            }
    }

    // Use this for initialization
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }


}

