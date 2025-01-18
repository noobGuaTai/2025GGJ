using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Tween;


public class Tween : MonoBehaviour
{
	// Start is called before the first frame update
	void Start() {

	}

	public enum TweenType
	{
		UNKNOWN = -1,
		FLOAT,
		VECTOR2,
		VECTOR3,
		VECTOR2INT,
		VECTOR3INT,
	}

	public enum TweenState
	{
		STOP,
		RUNNING,
		PAUSED,
	}

	public enum PlayMode
	{
		NORMAL,
		REPEAT,
		REVERSE,
	}

	abstract public class TweenNodeBase
	{
		public float time;
		public TransitionType transitionType = TransitionType.LINEAR;
		public EaseType easeType = EaseType.IN;
		public TweenTrack master;
		abstract public void Call(float alpha);
	}


	[System.Serializable]
	public class TweenTrack
	{
		public string name;
		public TweenState _tweenState;

		public int tweenIndex = 0;
		public float tweenTime = 0;
		PlayMode _playMode = PlayMode.NORMAL;
		public PlayMode playMode {
			set {
				_playMode = value;
			}
			get {
				return _playMode;
			}
		}
		public float totalTime{
			get => tweenNodeList.Sum(x => x.time);
		}
		public TweenTrack SetPlayMode(PlayMode value) { playMode = value; return this; }
		public bool clearWhenEnd = true;
		public TweenTrack SetClearWhenEnd(bool value) { clearWhenEnd = value; return this; }
		public TweenNodeBase lastActiveTween;

		public List<TweenNodeBase> tweenNodeList = new List<TweenNodeBase>();
		public static TweenTrack defaultTrack = new TweenTrack { name = "__default" };
		public static implicit operator TweenTrack(int index) {
			if (index == -1)
				return defaultTrack;
			else return new TweenTrack { name = "__track_" + index };
		}
		public static TweenTrack Null2Default(TweenTrack track) => track != null ? track : defaultTrack;
		public void Play() {
			if (_tweenState == Tween.TweenState.RUNNING || tweenNodeList.Count == 0)
				return;

			_tweenState = Tween.TweenState.RUNNING;
			lastActiveTween = null;

			if (playMode == PlayMode.REVERSE) {
				tweenTime = tweenNodeList.Last().time;
				tweenIndex = tweenNodeList.Count - 1;
			}
			else {
				tweenIndex = 0;
				tweenTime = 0;
			}
		}
		public void Clear(){
			_tweenState = TweenState.STOP;
			tweenNodeList.Clear();
		}
		public bool isPlaying => _tweenState == TweenState.RUNNING;

		
	}
	public Dictionary<string, TweenTrack> tweenDict = new Dictionary<string, TweenTrack>();
	public TweenTrack defaultTrack {
		get {
			if (!tweenDict.TryGetValue("__default", out var ret)) {
				ret = new TweenTrack() { name="__default"};
				tweenDict.Add("__default", ret);
			}
			return ret;
		}
	}
	public void Clear(string trackName = "__default", bool noWarning = false) {
		if (tweenDict.TryGetValue(trackName, out var track)) {
			track.Clear();
		}
		else {
			if (!noWarning)
				Debug.LogWarning($"Tween.Clear: track named({trackName}) not find");
		}
	}
	public void ClearAll() => tweenDict.ToList().ForEach(x => x.Value.Clear());
	public void Play() {
		foreach (var pair in tweenDict) {
			pair.Value.Play();
		}
	}

	public void Play(TweenTrack track) {
		track.Play();
	}

	public void Play(string trackName) {
		if (tweenDict.TryGetValue(trackName, out var track)) {
			Play(track);
			return;
		}
		Debug.LogError("Tween: Unkown track named:" + trackName);
	}
	public void Play(params string[] trackNames) {
		foreach (var p in trackNames)
			Play(p);
	}

	public void Stop() {
		foreach (var pair in tweenDict) {
			var track = pair.Value;
			ref var _tweenState = ref pair.Value._tweenState;
			var tweenNodeList = pair.Value.tweenNodeList;
			_tweenState = Tween.TweenState.STOP;
			if (track.playMode == PlayMode.REVERSE) {
				track.tweenTime = tweenNodeList.Last().time;
				track.tweenIndex = tweenNodeList.Count - 1;
			}
			else {
				track.tweenIndex = 0;
				track.tweenTime = 0;
			}

		}
	}

	public void Pause() {
		foreach (var pair in tweenDict) {
			ref var _tweenState = ref pair.Value._tweenState;
			_tweenState = Tween.TweenState.PAUSED;
		}
	}

	//void tweenCall<T>(TweenValueNode<T> tweenNode, float alpha)
	//{
	//    var cntT = (tweenNode.end - tweenNode.start) * alpha + tweenNode.start;
	//    tweenNode.setter(cntT);
	//}

	void TweenProcess(TweenTrack track, TweenNodeBase tweenNodeBase, float alpha) {
		tweenNodeBase.Call(alpha);
		track.lastActiveTween = tweenNodeBase;
	}


	// Update is called once per frame
	public void Update() {
		foreach (var pair in tweenDict.ToList()) {
			var track = pair.Value;
			ref var _tweenState = ref pair.Value._tweenState;
			var tweenNodeList = pair.Value.tweenNodeList;
			if (_tweenState != TweenState.RUNNING)
				continue;

			var delTime = Time.deltaTime;
			if (track.playMode == PlayMode.REVERSE)
				track.tweenTime -= delTime;
			else
				track.tweenTime += delTime;

			Action loopProcess = () => {
				if (track.playMode == PlayMode.REVERSE) {
					while (track.tweenIndex >= 0 && track.tweenTime <= (
						track.tweenIndex == 0 ? 0 :
						tweenNodeList[track.tweenIndex - 1].time)) {
						TweenProcess(track, tweenNodeList[track.tweenIndex], 0);
						track.tweenIndex--;
					}
					return;
				}
				while (track.tweenIndex < tweenNodeList.Count && track.tweenTime >= tweenNodeList[track.tweenIndex].time) {
					TweenProcess(track, tweenNodeList[track.tweenIndex], 1);
					track.tweenIndex++;
				}
			};
			loopProcess();
			Func<bool> endCondition = () => {
				if (track.playMode == PlayMode.REVERSE) {
					return track.tweenIndex == -1;
				}
				else {
					return track.tweenIndex == tweenNodeList.Count;
				}
			};

			if (endCondition()) {
				switch (track.playMode) {
					case PlayMode.NORMAL:
						_tweenState = TweenState.STOP;
						if (track.clearWhenEnd)
							Clear(track.name);
						continue;
					case PlayMode.REPEAT:
						track.tweenIndex = 0;
						track.tweenTime -= tweenNodeList.Last().time;
						loopProcess();
						continue;
					case PlayMode.REVERSE:
						_tweenState = TweenState.STOP;
						if (track.clearWhenEnd)
							Clear(track.name);
						continue;
				}

			}

			float preTime = 0;
			if (track.tweenIndex > 0)
				preTime = tweenNodeList[track.tweenIndex - 1].time;
			float cntAlpha = (track.tweenTime - preTime) / (tweenNodeList[track.tweenIndex].time - preTime + 1e-6f);
			var transitionType = tweenNodeList[track.tweenIndex].transitionType;
			var easeType = tweenNodeList[track.tweenIndex].easeType;

			cntAlpha = EaseAndTrainsitionProcess(cntAlpha, easeType, transitionType);

			TweenProcess(track, tweenNodeList[track.tweenIndex], cntAlpha);
		}
	}

	public enum TransitionType
	{
		LINEAR,
		SIN,
		QUAD,
		CUBIC,
		QUART,
		QUINT,
		EXP,
		BACK,
		CIRC,
	}

	// return x that f(x)=0.5
	static float GetTransitionTypeInoutFactor(TransitionType type) {
		switch (type) {
			case TransitionType.LINEAR:
				return 0.5f;
			case TransitionType.SIN:
				return 0.333333333f;
			case TransitionType.QUAD:
				return 0.707107f;
			case TransitionType.CUBIC:
				return 0.793701f;
			case TransitionType.QUART:
				return 0.840896f;
			case TransitionType.QUINT:
				return 0.870551f;
			case TransitionType.EXP:
				return 0.900141f;
			case TransitionType.BACK:
				return 0.8728f;
			case TransitionType.CIRC:
				return 0.866025f;
			default:
				Debug.LogError("Unknown transition type");
				return -1;
		}
	}
	public enum EaseType
	{
		IN,
		OUT,
		IN_OUT,
		OUT_IN,
	}
	public static float TransitionProcess(float alpha, TransitionType type) {
		switch (type) {
			case TransitionType.LINEAR:
				return alpha;
			case TransitionType.SIN:
				return Mathf.Sin(alpha * 0.5f * Mathf.PI);
			case TransitionType.QUAD:
				return alpha * alpha;
			case TransitionType.CUBIC:
				return alpha * alpha * alpha;
			case TransitionType.QUART:
				return alpha * alpha * alpha * alpha;
			case TransitionType.QUINT:
				return alpha * alpha * alpha * alpha * alpha;
			case TransitionType.EXP:
				return (Mathf.Pow(2, 10 * alpha - 10) - Mathf.Pow(2, -10)) * (1 / (1 - Mathf.Pow(2, -10)));
			case TransitionType.BACK:
				const float c1 = 1.70158f;
				const float c3 = c1 + 1;
				return c3 * alpha * alpha * alpha - c1 * alpha * alpha;
			case TransitionType.CIRC:
				return 1 - Mathf.Sqrt(1 - alpha * alpha);
		}
		Debug.LogError("Unknow transition!");
		return -1;
	}

	public static float EaseAndTrainsitionProcess(
		float alpha, EaseType easeType, TransitionType transitionType) {
		var factor = 0.0f;
		switch (easeType) {
			case EaseType.IN:
				return TransitionProcess(alpha, transitionType);
			case EaseType.OUT:
				return 1 - TransitionProcess(1 - alpha, transitionType);
			case EaseType.IN_OUT:
				factor = GetTransitionTypeInoutFactor(transitionType) * 2; // equal to divide 0.5
				if (alpha < 0.5f) {
					return TransitionProcess(factor * alpha, transitionType);
				}
				else {
					return 1 - TransitionProcess(factor * (1 - alpha), transitionType);
				}
			case EaseType.OUT_IN:
				factor = GetTransitionTypeInoutFactor(transitionType) * 2; // equal to divide 0.5
				if (alpha < 0.5f) {
					return 0.5f - TransitionProcess(factor * (1 - alpha), transitionType);
				}
				else {
					return 0.5f + TransitionProcess(factor * alpha, transitionType);
				}
		}
		Debug.LogError("Unknow transition!");
		return -1;
	}

	public bool TryGetTrack(string name, out TweenTrack track) {
		if (tweenDict.TryGetValue(name, out var t)) {
			track = t;
			return true;
		}
		track = null;
		return false;
	}

	public bool AnyPlaying { get {
			foreach (var t in tweenDict.Values) {
				if (t.isPlaying) return true;
			}
			return false;
		} 
	}
}