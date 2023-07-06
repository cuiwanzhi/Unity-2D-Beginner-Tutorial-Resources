/*
 * @Description: 单例基类
 * @Author: cuiwanzhi
 * @Date: 2023-03-31 03:41:52
 * @FilePath: /2D Beginner Tutorial Resources/Assets/Script/Manager/SingleBase.cs
 */

using System;

public abstract class SingleBase<T> where T : SingleBase<T> {
	private static T _instance = null;
	public static T Instance {
		get {
			if (_instance is not null) return _instance;
			_instance = Activator.CreateInstance<T>();
			return _instance;
		}
		private set {}
	}

	protected SingleBase() { }
}