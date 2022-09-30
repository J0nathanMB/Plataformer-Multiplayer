using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class playerAttack : MonoBehaviourPunCallbacks
{
    #region Variaveis

    //Unity
    public PhotonView phview;

    Vector3 mousepos;
    Vector3 screenPoint;
    Vector2 Offset;
    public float lookAng;
    public int side;

    public Transform firePosition;
    Quaternion rot;

    public GameObject Player;
    public bool reset;

    //Controle
    float countFireball;
    bool canFireball;
    public float mpTotal, mpAtual;
    public FillControler mpFill;

    #endregion

    //ControleUI
    public Image mpBar;
    //public GameObject myCanvasVar;
    public bool UISet;


    void Start()
    {
       
        countFireball = 0;
        canFireball = true;
        mpAtual = mpTotal = 100;
        UISet = false;
        reset = false;
        side = 0;
    }

    // Update is called once per framea
    void Update()
    {
        if (phview.IsMine)
        {
            //if (!UISet)
            //{
            //    //SetUI();
            //    UISet = true;
            //}
            UpdateSkills();
            RestoreMP();
            LookMouse();
            Fireball();

            UpdateUI();
        }
        
    }

    void LookMouse()
    {
        if (Player.GetComponent<playerMovement>().side != 1)
        {
            mousepos = Input.mousePosition;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Offset = new Vector2(mousepos.x - screenPoint.x, mousepos.y - screenPoint.y);

            lookAng = Mathf.Atan2(Offset.y, Offset.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, lookAng);
            rot = transform.rotation;
        }
        else if (Player.GetComponent<playerMovement>().side != 0)
        {
            mousepos = Input.mousePosition;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Offset = new Vector2(mousepos.x - screenPoint.x, mousepos.y - screenPoint.y);

            lookAng = Mathf.Atan2(Offset.y, Offset.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, lookAng + 180f);
            rot = transform.rotation;
        }
    }

    void Fireball()
    {
        if (Input.GetMouseButton(0) && canFireball && mpAtual > 10)
        {
            if (Player.GetComponent<playerMovement>().side == 1)
            {
                PhotonNetwork.Instantiate("Fireball", firePosition.position, transform.rotation = Quaternion.Euler(0, 0, lookAng));
            }
            else
            {
                PhotonNetwork.Instantiate("Fireball", firePosition.position, rot);
            }

            countFireball = 1.5f;
            canFireball = false;
            mpAtual -= 10;
        }
    }

    void RestoreMP()
    {
        if (mpAtual >= mpTotal)
        {
            mpAtual = mpTotal;
            return;
        }
        else
        {
            mpAtual += 5 * Time.deltaTime;
        }
    }
    void UpdateSkills()
    {

        if (!canFireball)
        {
            countFireball -= 1 * Time.deltaTime;
        }
       
        if (countFireball <= 0)
        {
            canFireball = true;
        }
    }

    //void SetUI()
    //{
    //    //mana bar
    //    //mpBar = myCanvasVar.transform.GetChild(1).transform.GetChild(1)
    //    //    .transform.GetChild(0).gameObject.GetComponent<Image>();
    //}

    void UpdateUI()
    {
        int aux = (int)mpAtual;

        mpFill.SetfillAmount (mpAtual / mpTotal);
        



    }

}
