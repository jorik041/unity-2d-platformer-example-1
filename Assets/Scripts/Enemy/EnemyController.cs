using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	
	// 歩く速度が設定される
	public float WalkVelocity;

	// プレイヤーなど直接ダメージを受けるタグが設定される
	public string PlayerTag;

	public bool IsDestroyed { get; set; }

	private Rigidbody2D Body { get; set; }

	void Awake()
	{
		Body = GetComponent<Rigidbody2D>(); // オブジェクトにアタッチされているRigidbody2Dのコンポーネントを取得
		IsDestroyed = false;
	}


	void FixedUpdate()
	{
		// 歩ける場合は"設定された歩く速度"に、そうでない場合は"0の速度に"設定する
		Body.velocity = new Vector2(
			CanWalk() ? WalkVelocity : 0,
			Body.velocity.y
		);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.gameObject.tag.Equals(PlayerTag) && IsUnderPlayer())
		{
			IsDestroyed = true;
			Destroy(this.gameObject);
		}
	}

	private bool IsUnderPlayer()
	{
		var hits = Physics2D.LinecastAll(
			transform.position,
			transform.position + transform.up * 1.1f
		);
		foreach (var hit in hits)
		{
			if (hit.transform.gameObject.tag.Equals(PlayerTag))
			{
				return true;
			}
		}
		return false;
	}

	private bool CanWalk()
	{
		return true; // 仮
	}

}