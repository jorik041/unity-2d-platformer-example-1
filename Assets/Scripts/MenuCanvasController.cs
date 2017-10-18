
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvasController : MonoBehaviour {

	public string TitleSceneName;
	
	public void OnClickBackToTitle()
	{
		SceneManager.LoadScene(TitleSceneName);		
	}

}
