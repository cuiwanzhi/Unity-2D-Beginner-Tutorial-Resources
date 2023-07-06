using UnityEngine;
using UnityEngine.UI;

public class HeadUI : MonoBehaviour {

	/// <summary>
	/// 血量控制对象
	/// </summary>
	public GameObject headUI;

	/// <summary>
	/// 血量对象image组件
	/// </summary>
	private Image image;
	
	/// <summary>
	/// 主角对象
	/// </summary>
	private RubyController ruby;

	// Start is called before the first frame update
	void Start() {
		this.image = this.headUI.GetComponent<Image>();
		this.image.fillAmount = 1;
		this.ruby = GameObject.Find("Ruby").GetComponent<RubyController>();
	}

	// Update is called once per frame
	void Update() {
		// 每帧获取主角当前血量
		if (this.ruby == null || this.ruby.gameObject.activeSelf == false) {
			return;
		}
		// 需要强行转成float，否则会出现0.0
		this.image.fillAmount = (float)this.ruby.currentHealth / (float)this.ruby.maxHealth;
	}
}
