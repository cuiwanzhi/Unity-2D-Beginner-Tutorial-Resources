/*
 * @Description: 子弹管理类单例
 * @Author: cuiwanzhi
 * @Date: 2023-03-31 03:29:39
 * @FilePath: /2D Beginner Tutorial Resources/Assets/Script/Manager/PoolManagerBase.cs
 */
#nullable enable

using System;
using System.Collections.Generic;
using ConfigNamespace;
using UnityEngine;

/// <summary>
/// 对象池基类
/// </summary>
/// <typeparam name="K">子类自己，用于传递给SingleBase</typeparam>
/// <typeparam name="T">模板预制体的关键组件，继承自PoolItemBase</typeparam>
public abstract class PoolManagerBase<K, T> : SingleBase<K>
where K : PoolManagerBase<K, T>
where T : PoolItemBase {

	/// <summary>
	/// 当前生成的子弹总数量
	/// </summary>
	public int currProjectNum = 0;

	/// <summary>
	/// 对象池
	/// </summary>
	private Queue<T> listItem;
	/// <summary>
	/// 预制体模板
	/// </summary>
	protected GameObject template;

	/// <summary>
	/// 初始化方法，子类不要开放调用权限，保持protected。
	/// </summary>
	/// <exception cref="Exception"></exception>
	protected PoolManagerBase() {
		this.listItem = new Queue<T>();
		// 初始化获取预制体
		if (this.template == null) {
			var template = this.GetTemplate();
			this.template = template;
		}
	}

	/// <summary>
	/// 根据GetPrefabPath返回的路径获取预制体GameObject对象
	/// </summary>
	/// <returns>GameObject对象</returns>
	/// <exception cref="Exception">如果没有找到就会触发异常</exception>
	protected GameObject GetTemplate() {
		GameObject template = Resources.Load<GameObject>(this.GetPrefabPath());
		if (template is null) {
			throw new Exception($"{typeof(GameObject)} has no prefab in path: {this.GetPrefabPath()}");
		}
		return template;
	}

	/// <summary>
	/// 获取一个子弹预制体的T组件,如果达到上限了就返回The requested operation caused a stack overflow.null
	/// </summary>
	/// <returns>返回一个T组件,如果达到上限了就返回null</returns>
	public T? GetItem() {
		if (!this.listItem.TryDequeue(out T? item)) {
			item = this.CreateItem();
		}
		item?.ResetState();
		return item;
	}

	/// <summary>
	/// 回收对象,回收前会调用onRetrieve方法
	/// </summary>
	/// <param name="item">需要回收的对象的继承自PoolItemBase的T组件</param>
	public void Retrieve(T item) {
		item.onRetrieve();
		listItem.Enqueue(item);
		Debug.Log(this.listItem.Count);
	}

	/// <summary>
	/// 创建一颗子弹对象
	/// </summary>
	/// <returns>返回创建的子弹对象的Projectile组件</returns>
	private T? CreateItem() {
		if (this.currProjectNum >= this.getMaxItemNum()) {
			return null;
		}
		this.currProjectNum++;
		GameObject gameObject = UnityEngine.Object.Instantiate(this.template);
		return gameObject.GetComponent<T>();
	}

	// ========抽象方法=============//

	/// <summary>
	/// 获得预制体的路径类似"Art/Prefabs/Projectile"
	/// </summary>
	/// <param name="param">这个参数是为了让子类更好的重写而不重载。</param>
	/// <returns></returns>
	protected abstract string GetPrefabPath();

	/// <summary>
	/// 获取当前最多能存在的的Item数量，由子类覆盖，默认不会限制数量
	/// </summary>
	/// <returns>最大数量</returns>
	protected abstract int getMaxItemNum();
}
