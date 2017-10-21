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

    public string GoalTag;

    // クリア時に表示するPrefabが設定される
    public GameObject ClearOverlayPrefab;

    public GameObject GameoverOverlayPrefab;
    
    private Rigidbody2D Body { get; set; }

    private Animator PlayerAnimator { get; set; }

    private SpriteRenderer PlayerRenderer { get; set; }

    // 生成したOverlayが格納される
    private GameObject ClearOverlay = null;

    private GameObject GameoverOverlay = null;
    
    // `Awake`はオブジェクトが読み込まれた際に実行されるメソッドです
    void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerRenderer = GetComponent<SpriteRenderer>();
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
            IsWalking = isWalking;

            if (velocityX != 0)
            {
                bool isRight = velocityX > 0;
                PlayerRenderer.flipX = !isRight;
            }
        }

        if (CanJump() && IsJumpPressed())
        {
            // `Rigidbody2D`に上方向の力を加えます
            Body.AddForce(Vector2.up * JumpForce);
            IsJumping = true;
        }

        if (!IsGrounded())
        {
            IsUp = Body.velocity.y > 0;
            IsDown = !IsDown;
        }
        else
        {
            IsUp = false;
            IsDown = false;
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
        if (other.gameObject.tag.Equals(EnemyTag) && !IsPhysicalAttack())
        {
            if (!other.gameObject.GetComponent<EnemyController>().IsDestroyed)
            {
                IsDamaged = true; // ダメージを受けたことをフラグで設定する
                OnDamaged();
            }
        } else if (other.gameObject.tag.Equals(GoalTag))
        {
            OnTouchGoal(other);
        }
        if (IsGrounded())
        {
            IsJumping = false;
        }
    }

    private void OnTouchGoal(Collision2D other)
    {
        Destroy(other.gameObject);

        if (ClearOverlay == null)
        {
            ClearOverlay = Instantiate(ClearOverlayPrefab);
        }
    }

    private void OnDamaged()
    {
        Body.AddForce(Vector2.up * (JumpForce * 2));
        if (GameoverOverlay == null)
        {
            GameoverOverlay = Instantiate(GameoverOverlayPrefab);
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

    private bool IsPhysicalAttack()
    {
        // プレイヤーの下に存在するオブジェクトを全て取得する
        var hits = Physics2D.LinecastAll(
            transform.position,
            transform.position - transform.up * 1.1f
        );
        foreach (var hit in hits)
        {   // ひとつずつチェックし、`EnemyTag`と一致するものが存在した場合は中断し`true`を返す
            if (hit.transform.gameObject.tag.Equals(EnemyTag))
            {
                return true;
            }
        }
        return false; // ひとつも`EnemyTag`を持つオブジェクトが存在しない場合`false`を返す
    }

    // 歩けるか否か
    private bool CanWalk()
    {
        return !IsDamaged; // `IsDamaged`のフラグを`!`で反転させる
    }

    private bool IsWalking
    {
        get { return PlayerAnimator.GetBool("IsWalking"); }
        set { PlayerAnimator.SetBool("IsWalking", value); }
    }

    private bool IsJumping
    {
        get { return PlayerAnimator.GetBool("IsJumping"); }
        set { PlayerAnimator.SetBool("IsJumping", value); }
    }

    private bool IsUp
    {
        get { return PlayerAnimator.GetBool("IsUp"); }
        set { PlayerAnimator.SetBool("IsUp", value); }
    }
    
    private bool IsDown {
        get { return PlayerAnimator.GetBool("IsDown"); }
        set { PlayerAnimator.SetBool("IsDown", value); }
    }
    
    private bool IsDamaged {
        get { return PlayerAnimator.GetBool("IsDamaged"); }
        set { PlayerAnimator.SetBool("IsDamaged", value); }
    }

}
