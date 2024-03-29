using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    
    CharacterController controller;
    Animator animator;
    
    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;
    
    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    public float accelerationZ;
    
    // ライフ取得用関数
    public int Life()
    {
        return life;
    }
    
    // 気絶判定
    public bool IsStan()
    {
        return recoverTime > 0.0f || life <= 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // 必要なコンポーネントを取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ用
        if(Input.GetKeyDown("left")) MoveToLeft();
        if(Input.GetKeyDown("right")) MoveToRight();
        if(Input.GetKeyDown("space")) Jump();
        
        if(IsStan())
        {
            // 動きを止め、気絶状態からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {
            // 徐々に加速しZ方向に常に前進させる
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);
        
            // X方向は目標のポジションまでの差分の割合で速度を計算
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }
        
        // 重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;
        
        // 移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);
            
        // 移動後接地していたらY方向の速度はリセットする
        if(controller.isGrounded) moveDirection.y = 0;
            
        // 速度が0以上なら「走っているフラグ」をtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);
    }
    
    // 左のレーンに移動を開始
    public void MoveToLeft()
    {
        if(IsStan()) return;
        if(controller.isGrounded && targetLane > MinLane) targetLane--;
    }
    
    // 右のレーンに移動を開始
    public void MoveToRight()
    {
        if(IsStan()) return;
        if(controller.isGrounded && targetLane < MaxLane) targetLane++;
    }
    
    // ジャンプ
    public void Jump()
    {
        if(IsStan()) return;
        if(controller.isGrounded)
        {
            moveDirection.y = speedJump;
            animator.SetTrigger("jump");
        }
    }
    
    // CharacterControllerにコリジョンが生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(IsStan()) return;
        
        if(hit.gameObject.tag == "Robo")
        {
            // ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;
            
            // ダメージトリガーを設定
            animator.SetTrigger("damage");
            
            // ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }
    }
}
