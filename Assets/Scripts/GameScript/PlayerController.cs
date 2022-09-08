using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed; //歩く速度
    public GroundChecker ground; //キャラ設置フラグ
    public GameOver gover; //ゲームオーバーフラグ
    GameObject effect,effectBox = null; //エフェクト群
    Animator anim,animEffect; //アニメ群
    Rigidbody2D rb; //rigibdoby
    bool isJump = false; //ジャンプの有無
    float deltaT = 0.0f; //歩量測定
    public bool isMove = true; //動きの許可

    static int entryState = Animator.StringToHash("Base Layer.EntryState");

    void Start()
    {
        effect = (GameObject)Resources.Load("Effect1");
        animEffect = effect.GetComponent<Animator>();
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(isMove)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == entryState || 
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                Attack();
                //Jump();
                Walk();
            }
        }
    }
    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("Attack1");
            effectBox = Instantiate(effect);
            effectBox.transform.SetParent(this.gameObject.transform);
            Destroy(effectBox,1.2f);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("Attack_p1");
        }
    }
    void Jump()
    {
        bool isGround = ground.IsGround();
        float yMove = Input.GetAxis("Vertical");

        if(rb.velocity.y>1){
            anim.SetBool("Up",true);
            anim.SetBool("Down",false);
        }
        else{
            anim.SetBool("Up",false);
            anim.SetBool("Down",true);
        }

        if(isGround)
        {
            anim.SetBool("Up",false);
            anim.SetBool("Down",false);
            if(yMove>0)
            {
                rb.velocity = new Vector2(rb.velocity.x,6f);
                isJump = true;
            }
            else{
                if(isJump){
                    anim.SetBool("Land", true);
                    isJump = false;
                }
                else
                {
                    anim.SetBool("Land", false);
                }
            }
        }
    }
    void Walk()
    {
        deltaT += Time.deltaTime; //走りカウント
        float xMove = Input.GetAxis("Horizontal");

        if(deltaT >= 0.1f)
        {
            
        }  

        if(xMove>0)
        {
            rb.velocity = new Vector2(speed,rb.velocity.y);
            anim.SetBool("Walk",true);
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if(xMove<0)
        {
            rb.velocity = new Vector2(-speed,rb.velocity.y);
            anim.SetBool("Walk",true);
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
            anim.SetBool("Walk",false);
        }
    }
    void Run()
    {
             

    }
    public void PlayerDead()
    {
        anim.SetBool("Walk",false);
        anim.SetBool("Dead",true);
        isMove = false;
        float x = Random.Range(-20f,20f);
        float y = Random.Range(-20f,20f);
        rb.velocity = new Vector2(x,y);
        gover.Dead();
    }
}
