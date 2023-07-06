using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

	/// <summary>
	/// NPC对话框
	/// </summary>
	public NPCDialog dialog;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void ShowDialog(RubyController rubyController) {
		Debug.Log("OnHit");
		this.dialog.Show($@"少年，我看你骨骼惊奇，是万中无一的练武奇才。我这里有本秘籍《如来神掌》。我看与你有缘，就十块钱卖给你吧！等等，没钱？那你先帮我把旁边坏掉的机器人修好，我就免费送给你！少年，维护世界和平就靠你了~~~"
		);
	}
}
