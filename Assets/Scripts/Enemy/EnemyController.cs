using UnityEngine;

public class EnemyController : MonoBehaviour {

	// 歩く速度が設定される
	public float WalkVelocity;

	private Rigidbody2D Body { get; set; }

	void Awake()
	{
		Body = GetComponent<Rigidbody2D>(); // オブジェクトにアタッチされているRigidbody2Dのコンポーネントを取得
	}


	void FixedUpdate()
	{
		// 歩ける場合は"設定された歩く速度"に、そうでない場合は"0の速度に"設定する
		Body.velocity = new Vector2(
			CanWalk() ? WalkVelocity : 0,
			Body.velocity.y
		);
	}

	private bool CanWalk()
	{
		return true; // 仮
	}

}