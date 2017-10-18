using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePanelController : MonoBehaviour
{

	// 遷移先のシーン名が設定される
	public string GameSceneName;
	
	// ボタンをクリックした時のイベントとして接続されるメソッド
	public void OnClickStart()
	{
		SceneManager.LoadScene(GameSceneName); // 設定されたシーンへ遷移する
	}

}
