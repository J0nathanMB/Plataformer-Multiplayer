using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace ProjectPsy
{

    public class SimpleEnemy : MonoBehaviour
    {
        #region Variáveis

        [Header("Componentes")]
        [SerializeField] PhotonView phview;
        [SerializeField] Rigidbody2D rgbody;
        //[SerializeField] LayerMask wallLayer;

        [Header("Atributos")]
        [SerializeField] float speed;
        public int vida;
        int side;

        [Header("Controle")]

        [SerializeField] GameObject Points;
        Vector3 movement;
        public float timeInAir;
        public bool inAir;
        public bool flipou;
        #endregion
        void Start()
        {
            speed = 4;
            side = -1;
            flipou = true;
            vida = 100;
        }

        //Update is called once per frame
        void Update()
        {
            if (phview.IsMine)
            {

                Move();
            }
        }

        private void Move()
        {
            if(!inAir)
            {

                movement = new Vector2(side, 0);
                transform.position += movement * Time.deltaTime * speed;
            }
            else
            {
                Levitate();
            }

        }

        void Flip()
        {
            //transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

            if (side == 1 && !flipou)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                side = -1;
                flipou = true;
            }
            else if (side == -1 && flipou) 
            {
                transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
                side = 1;
                flipou = false;
            }
        }

        //bool IsDied()
        //{
        //}

        void Levitate()
        {  
            if(timeInAir <= 1)
            {
                rgbody.gravityScale = -0.3f;
            }
            else if(timeInAir > 1 && timeInAir < 2)
            {
                rgbody.gravityScale = 0;
            }
            else if (timeInAir > 2)
            {
                rgbody.gravityScale = 1;
                inAir = false;
            }
            timeInAir += 1 * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "TerritoryMarking")
            {
                Flip();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                if(collision.gameObject.GetComponent<playerMovement>())
                {
                    collision.gameObject.GetComponent<playerMovement>().TakeDamage(20);
                }
               
            }
        }

        [PunRPC]

        public void TakeDamage(int d)
        {
            vida -= d;

            if (vida <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
