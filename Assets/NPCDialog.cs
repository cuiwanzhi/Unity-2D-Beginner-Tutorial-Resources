using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// NPC对话框
/// </summary>
public class NPCDialog : MonoBehaviour {

	/// <summary>
	/// 显示时长
	/// </summary>
	public float showTime = 4f;
	public TextMeshProUGUI  textMesh;

	// Start is called before the first frame update
	void Start() {
		this.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		var hasInputSpace = Input.GetKeyDown(KeyCode.Space);
		if (hasInputSpace) {
			this.OnInputSpace();
		}
	}

	/// <summary>
	/// 空格键按下时，翻页
	/// </summary>
	void OnInputSpace() {
		if (!this.gameObject.activeSelf) {
			return;
		}
		this.CancelInvoke("hide");
		this.Invoke("hide", this.showTime);
		var maxPage = this.textMesh.textInfo.pageCount;
		var currentPage = this.textMesh.pageToDisplay;
		if (currentPage < maxPage) {
			this.textMesh.pageToDisplay += 1;
		} else {
			this.hide();
		}
	}

	/// <summary>
	/// 显示对话框
	/// </summary>
	/// <param name="showText">显示内容</param>
	/// <param name="showTime">可选参数，如果不指定，将使用默认时间</param>
	public void Show(string showText,float? showTime = null) {
		this.gameObject.SetActive(true);
		this.textMesh.text = showText;
		this.textMesh.pageToDisplay = 1;
		this.showTime = showTime ?? this.showTime;
		this.Invoke("hide", this.showTime);
	}

	/// <summary>
	/// 隐藏对话框
	/// </summary>
	public void hide() {
		this.gameObject.SetActive(false);
	}
}
