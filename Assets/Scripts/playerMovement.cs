using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class playerMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public Text nameText;

    public PhotonView pv;
    private Vector3 smoothMove;
    private GameObject sceneCamera;
    public GameObject playerCamera;
    public SpriteRenderer sr;
    public static GameObject LocalPlayerInstance;
    public static playerMovement instance;

    public int side;
    public bool UISet;

    public GameObject firePoint;
    public GameObject fireBall;

    public float lifeTotal, currentLife;
    public Image fillLife;
    //public GameObject myCanvas;

    public FillControler HpFill;

    


    // public int myNumberInroom;

    private void Awake()
    {
        //Grab references for rigidbody
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        
    }
    
    private void Start()
    {
        if (photonView.IsMine)
        {
            nameText.text = PhotonNetwork.NickName;
            sceneCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
            //SetUI();
        }
        
        side = 0;
        currentLife = 50;
       lifeTotal = 100;
    }

    private void Update()
    {

        if (photonView.IsMine)
        {
            ProcessInputs();

            RestoreHP();
            UpdateUI();
        }
        else
        {
            smoothMovement();
        }
       
    }
       
    private void smoothMovement()
    {

        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 1000);

    }
    private void ProcessInputs()
    {

            horizontalInput = Input.GetAxisRaw("Horizontal");

            //Flip player when moving left-right
            if (horizontalInput > 0.01f)
            {
            transform.localScale = Vector3.one;
            side = 1;
            pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
             }
              
            else if (horizontalInput < -0.01f)
            {
            transform.localScale = new Vector3(-1, 1, 1);
            side = 0;
            pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
            }


        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream,  PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 15);
                Debug.Log("wall");
            }
                

            wallJumpCooldown = 0;
        }
    }

    void Morrer()
    {
        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
       
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    
    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        sr.flipX = true;
    }
    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        sr.flipX = false;
    }
    [PunRPC]
    public void TakeDamage(int d)
    {
        currentLife -= d;
        if (currentLife <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    void SetUI()
    {

       

        //fillLife = myCanvas.transform.GetChild(1).transform.GetChild(2)
        //    .transform.GetChild(2).GetComponent<Image>();
       

    }

    void UpdateUI()
    {
        int aux = (int)currentLife;
        HpFill.SetfillAmount (currentLife / lifeTotal);
    }

    void RestoreHP()
    {
        if (currentLife >= lifeTotal)
        {
            currentLife = lifeTotal;
        }
        else
        {
            currentLife += 4 * Time.deltaTime;
        }
    }



}