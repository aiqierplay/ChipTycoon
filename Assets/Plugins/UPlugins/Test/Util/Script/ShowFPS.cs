/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ShowFPS.cs
//  Info     : 显示实时帧数
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Test
{
	/// <summary>
	/// FPS显示位置枚举
	/// </summary>
	public enum FPSLocType {
		LeftTop,
		LeftBotton,
		RightTop,
		RightBotton
	}

	public class ShowFPS : MonoBehaviour {
		[Tooltip("显示位置")]
		public FPSLocType Location = FPSLocType.LeftTop;
		[Tooltip("目标帧数")]
		public float TargetFrame = 60f;
		[Tooltip("低帧数阈值")]
		public float LowFrame = 25f;
		[Tooltip("显示字号")]
		public float FontSize = 32f;
		[Tooltip("正常颜色")]
		public Color ColorNormal = Color.green;
		[Tooltip("危险颜色")]
		public Color ColorWarning = Color.red;

		private float _updateInterval = 0.2f;
		private float _timer;
		private float _frames;
		private float _fps;
		private GUIStyle _style;
		private Rect _showRect;

		void Start() {
			_style = new GUIStyle();
		}

		void Update() {
			_timer += UnityEngine.Time.unscaledDeltaTime;
			_frames++;
			if (_timer >= _updateInterval)
			{
				_fps = _frames / _timer;
				_timer -= _updateInterval;
				_frames = _timer * _fps;
			}
		}

		void OnGUI() {
			// 按屏幕比例缩放
			var rate = Screen.height / 1080f;
			_style.fontSize = (int)(FontSize * rate);
			// 计算颜色渐变插值
			var delta = Mathf.Clamp((_fps - LowFrame) / (TargetFrame - LowFrame), 0, 1f);
			var color = Color.Lerp(ColorWarning, ColorNormal, delta);
			_style.normal.textColor = Color.Lerp(_style.normal.textColor, color, UnityEngine.Time.deltaTime * 5f);
			// 设置位置
			switch (Location)
			{
				case FPSLocType.LeftTop:
					_showRect = new Rect(10f * rate, 10f * rate, 100f * rate, FontSize * rate);
					break;
				case FPSLocType.LeftBotton:
					_showRect = new Rect(10f * rate, Screen.height - (FontSize + 10) * rate, 100f * rate, 20f * rate);
					break;
				case FPSLocType.RightTop:
					_showRect = new Rect(Screen.width - (FontSize * 5 + 10f) * rate, 10f * rate, 100f * rate, FontSize * rate);
					break;
				case FPSLocType.RightBotton:
					_showRect = new Rect(Screen.width - (FontSize * 5 + 10f) * rate, Screen.height - (FontSize + 10) * rate, 100f * rate, 20f * rate);
					break;
			}
			// 在屏幕左上角绘制
			GUI.Label(_showRect, "FPS: " + _fps.ToString("f2"), _style);
		}
	}
}
