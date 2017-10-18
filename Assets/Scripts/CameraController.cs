using UnityEngine;

public class CameraController : MonoBehaviour
{

	// Unity Editorでプレイヤーのオブジェクトを設定する
	public GameObject PlayerObj;
	
	// 自身の`Camera`コンポーネント
	// private Camera Self { get; set; }

	// オブジェクト初期化時に実行される
	void Awake()
	{
		// Self = GetComponent<Camera>();
	}

	// `Update`メソッドの後、つまりプレイヤーの挙動などが確定した後で実行される
	void LateUpdate()
	{
		if(CanUpdateCameraPosition()) {
			// プレイヤーオブジェクトの現在地を取得する
			var playerPos = PlayerObj.transform.position;
			
			// 自身のオブジェクトが持つpositionを変更する
			this.transform.position = new Vector3(
				playerPos.x, // プレイヤーのx位置
				playerPos.y, // プレイヤーのy位置
				this.transform.position.z // 直前の自身のz位置を引き継ぐ
			);
		}
	}

	// Cameraの位置を変更できるか否か
	bool CanUpdateCameraPosition()
	{
		return true; // 仮
	}

}
