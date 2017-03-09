using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GetAllChildren {
	//------------------------------------------------------------
	// 子供関係にあるオブジェクトを全て取得
	public static List<GameObject> GetAll(this GameObject obj) {
		List<GameObject> allChildren = new List<GameObject>();
		GetChildren(obj, ref allChildren);
		return allChildren;
	}
	static void GetChildren(GameObject obj, ref List<GameObject> allChildren) {
		Transform children = obj.GetComponentInChildren<Transform>();
		//子要素がいなければ終了
		if (children.childCount == 0) {
			return;
		}
		foreach (Transform ob in children) {
			allChildren.Add(ob.gameObject);
			GetChildren(ob.gameObject, ref allChildren);
		}
	}
}