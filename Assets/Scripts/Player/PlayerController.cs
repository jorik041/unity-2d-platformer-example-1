using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    // ジャンプ力。publicのプロパティはInspectorから設定できる。
    public float JumpForce;

    // 歩く速度が設定される
    public float WalkVelocity;

    // フィールドなど着地できるレイヤーが設定される
    public LayerMask GroundLayer;

    // 敵などダメージを受けるタグが設定される
    public string EnemyTag;
    
    private Rigidbody2D Body { get; set; }

    // プレイヤーが既にダメージを受けたか否か
    private bool IsDamaged { get; set; }

    // `Awake`はオブジェクトが読み込まれた際に実行されるメソッドです
    void Awake()
    {
        Body = GetComponent<Rigidbody2D>(); // オブジェクトにアタッチされているRigidbody2Dのコンポーネントを取得
        IsDamaged = false; // 初期化時点ではまだダメージを受けていない
    }

    // `Start`はオブジェクトがScene上で開始された際に実行されるメソッドです
    void Start()
    {
        // no-op
    }

    // `Update`は毎フレームのレンダリング前に実行されるメソッドです
    void Update()
    {
        // no-op
    }
    
    // `LateUpdate`は`Update`の後に実行されるメソッドです
    // `Update`で移動などが済んだ後のオブジェクトに対し何か実行する場合に便利です
    void LateUpdate()
    {
        // no-op
    }
    
    // `FixedUpdate`は物理挙動の更新前に実行されるメソッドです
    // `Update`と異なり、固定フレームレートで実行されます
    void FixedUpdate()
    {
        if (CanWalk())
        {
            float x = GetAxisX();
            bool isWalking = x != 0;

            float velocityX = isWalking ? x : 0;
            
            Body.velocity = new Vector2(velocityX * WalkVelocity, Body.velocity.y);
        }

        if (CanJump() && IsJumpPressed())
        {
            // `Rigidbody2D`に上方向の力を加えます
            Body.AddForce(Vector2.up * JumpForce);
        }
    }

    // 方向キーのX方向を取得
    private float GetAxisX()
    {
        // `CrossPlatformInputManager` はStandard Assetsに含まれるコンポーネントです。
        // プラットフォーム間での入力差異を吸収し、統一インターフェースで入力を取得できます。
        return CrossPlatformInputManager.GetAxis ("Horizontal");
    }

    // ジャンプボタンが押されたか
    private bool IsJumpPressed()
    {    
        // `GetButtonDown`はボタンが押された瞬間のみ`true`が返され、押下されている間は考慮しません
        return CrossPlatformInputManager.GetButtonDown("Jump");
    }

    // 2DのCollisionが何かに触れた際に実行される
    void OnCollisionEnter2D(Collision2D other)
    {
        // 接触したオブジェクトのタグが、設定されたタグと一致する場合
        if (other.gameObject.tag.Equals(EnemyTag))
        {
            IsDamaged = true; // ダメージを受けたことをフラグで設定する
        }
    }

    // ジャンプすることが可能か否か
    private bool CanJump()
    {
        return IsGrounded() && !IsDamaged;
    }

    // 接地しているか否か
    private bool IsGrounded()
    {
        return Physics2D.Linecast(
            transform.position,
            transform.position - transform.up * 1.1f,
            GroundLayer
        );
    }

    // 歩けるか否か
    private bool CanWalk()
    {
        return !IsDamaged; // `IsDestroyed`のフラグを`!`で反転させる
    }

}
