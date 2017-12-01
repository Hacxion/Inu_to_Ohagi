using UnityEngine;
using System.Collections;
/**
 * シングルトンクラス
 * こいつを親に使ってシングルトン化させる
 */
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	static T instance;

	public static T Instance {
		get {
			if(instance == null){
				instance = (T)FindObjectOfType(typeof(T));
				if(instance == null){
					GameObject go = new GameObject(typeof(T).ToString());
					instance = go.AddComponent<T>();
					DontDestroyOnLoad(go);
				}
			}
			return instance;
		}
	}
}
