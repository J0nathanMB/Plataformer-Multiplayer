using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;



    public class Projectile : MonoBehaviour
    {
        public PhotonView phview;

        public float speed = 10f;

        public Vector3 Movement;

        public GameObject Player1;

        void Start()
        {
        Player1 = GameObject.Find("Player1(Clone)");
        }

        void Update()
        {
            if (phview.IsMine)
            {
                if (!phview.Owner.IsMasterClient)
                {
                    foreach (Player p in PhotonNetwork.PlayerList)
                    {
                        if (p.IsMasterClient)
                        {
                            phview.TransferOwnership(p);
                        }
                    }
                }

                Movement.x = speed * Time.deltaTime;
                transform.Translate(Movement);

            }
        }

        private void Destruir()
        {
            PhotonNetwork.Destroy(phview);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.name + " + " + collision.tag);
            if (collision.tag == "Inimigo")
            {
                collision.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, 50);
                Destruir();

            }

            else if (collision.tag == "Ground" || collision.tag == "Wall")
            {
                Destruir();
            }
        }




    }


