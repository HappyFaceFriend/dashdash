using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidShare : MonoBehaviour {
	string subject;
	string bodyHead;
	string body = "https://play.google.com/store/apps/details?id=com.testgame.game";

	public void Start()
	{
		int score = DataManager.Instance.recentScore;
		bool isHighScore = DataManager.Instance.recentScore == DataManager.Instance.highScore;
		if(Funcs.IsKorean())
		{
			if(isHighScore)
				subject = "쿵쿵 슬라임에서 " + score + "점으로 최고기록을 달성했어요!";
			else
				subject = "쿵쿵 슬라임에서 " + score + "점을 달성했어요!";
			bodyHead = "나도 하기 : ";
		}
		else
		{
			subject = "Just reached " + score + "points at Stompy Slime!";
			bodyHead = "Download : ";
		}
	}
	public void Share() {
#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent")) 
		using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent")) {
			intentObject.Call<AndroidJavaObject>("setAction", intentObject.GetStatic<string>("ACTION_SEND"));
			intentObject.Call<AndroidJavaObject>("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_SUBJECT"), subject);
			intentObject.Call<AndroidJavaObject>("putExtra", intentObject.GetStatic<string>("EXTRA_TEXT"), bodyHead + body);
			using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity")) 
			using (AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via"))
			currentActivity.Call("startActivity", jChooser);
		}
#endif
	}
}