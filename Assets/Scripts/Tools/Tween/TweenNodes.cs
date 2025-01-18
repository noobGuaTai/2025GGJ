using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tween;
using UnityEngine.Timeline;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

static public class VectorExtension
{
    public static Vector2Int ToIntVector(this Vector2 self) { return new Vector2Int((int)self.x, (int)self.y); }
    public static Vector3Int ToIntVector(this Vector3 self) { return new Vector3Int((int)self.x, (int)self.y, (int)self.z); }
}

public class TypeWrapper { }

public class TypeWrapperT<T> : TypeWrapper { 
    public T x;
    public TypeWrapperT(T x) { this.x = x; }
    public static T To<U>(U x) { 
        return ((TypeWrapperT<T>)(TypeWrapper)(new TypeWrapperT<U>(x))).x;
    }
}


public class TweenUtils {
    public static Dictionary<Type, TweenOperator> typeOP = new() { };
    static TweenCompileOperator[] types;
    static U Convert<T, U>(Func<T> q, Func<T, T> a, Func<T, U> qi) => qi(a(q()));
    static TweenUtils(){
        types = new TweenCompileOperator[] {
            new TweenCompileOperator<int>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(int)(x*y)),
            new TweenCompileOperator<float>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
            new TweenCompileOperator<double>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
            new TweenCompileOperator<Vector2>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
            new TweenCompileOperator<Vector3>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
            new TweenCompileOperator<Vector4>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
            new TweenCompileOperator<Vector2Int>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>Convert(() => new Vector2(x.x, x.y), z => z * y, z => new Vector2Int((int)z.x, (int)z.y))),
            new TweenCompileOperator<Vector3Int>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>Convert(() => new Vector3(x.x, x.y), z => z * y, z => new Vector3Int((int)z.x, (int)z.y))),
            new TweenCompileOperator<Color>((x,y)=>x-y,(x,y)=>x+y,(x,y)=>(x*y)),
        };
    }

    public static T Lerp<T>(T from, T to, float alpha){
        if(!typeOP.TryGetValue(typeof(T), out var op))
            throw new InvalidOperatorException($"Not supported T, ConciderCompile it", typeof(T));
        return op.Add(from, op.Ratio(op.Sub(to, from), alpha));
    }
    
}
public class TweenValueNode<T> : TweenNodeBase {
    public Action<T> setter;
    public T start;
    public T end;


    public TweenValueNode(Action<T> setter, T start, T end, float time) {
        this.setter = setter;
        this.start = start;
        this.end = end;
        base.time = time;
    }

    public override void Call(float alpha) {
        setter(TweenUtils.Lerp<T>(start, end, alpha));
    }
}
public class TweenGetterNode<T> : TweenNodeBase
{
    public Action<T> setter;
    public Func<T> getter;
    public T start;
    public T end;


    public TweenGetterNode(Action<T> setter, Func<T> getter, T end, float time) {
        this.setter = setter;
        this.getter = getter;
        this.end = end;
        base.time = time;

    }

    public override void Call(float alpha) {
        if (master.lastActiveTween != this)
            start = getter();
        setter(TweenUtils.Lerp<T>(start, end, alpha));
    }
}

public class TweenOffsetNode<T> : TweenNodeBase
{
    public Action<T> setter;
    public Func<T> getter;
    public T start;
    public T end;
    public T offset;

    public TweenOffsetNode(Action<T> setter, Func<T> getter, T offset, float time) {
        this.setter = setter;
        this.getter = getter;
        this.offset = offset;
        base.time = time;

    }

    public override void Call(float alpha) {
        if (master.lastActiveTween != this) { 
            start = getter();
            end = TweenUtils.typeOP[typeof(T)].Add<T>(start, offset);
        }
        setter(TweenUtils.Lerp<T>(start, end, alpha));
    }
}

public class TweenDynamicNode<T> : TweenNodeBase
{
    public Action<T> setter;
    public Func<T> from;
    public Func<T> to;
    public T start;
    public T end;
    public T offset;

    public TweenDynamicNode(Action<T> setter, Func<T> from, Func<T> to, float time) {
        this.from = from;
        this.to = to;
        this.setter = setter;
        base.time = time;
    }

    public override void Call(float alpha) {
        var start = from();
        var end = to();
        setter(TweenUtils.Lerp<T>(start, end, alpha));
    }
}


public static class TweenDynamicNodeUtil {

    static public TweenTrack AddTweenDynamic<T>(this Tween tween,  Action<T> setter, Func<T> from, Func<T> to, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN) {
        return AddTweenDynamic<T>(tween, "__default", setter, from, to, time, transitionType, easeType);
    }
	static public TweenTrack AddTweenDynamic<T>(this Tween tween, string trackName, Action<T> setter, Func<T> from, Func<T> to, float time,
		TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN) {
		if (!tween.tweenDict.TryGetValue(trackName, out var track)) {
			track = new TweenTrack { name = trackName };
			tween.tweenDict.Add(trackName, track);
		}
		
		if (track._tweenState == Tween.TweenState.RUNNING) {
			Debug.LogError("Try to call AddTween while tween is running");
			return track;
		}

		var cTime = 0f;
		var tweenNodeList = track.tweenNodeList;
		if (tweenNodeList.Count > 0)
			cTime = tweenNodeList[tweenNodeList.Count - 1].time;

		var tweenNode = new TweenDynamicNode<T>(setter, from, to, time + cTime);
		tweenNode.easeType = easeType;
		tweenNode.transitionType = transitionType;
		tweenNode.master = track;
		tweenNodeList.Add(tweenNode);
		return track;
	}
}
public abstract class TweenCompileOperator { }

public class TweenCompileOperator<T> : TweenCompileOperator{
    public TweenCompileOperator(Func<T,T,T> sub, Func<T,T,T> add, Func<T,float,T> ratio){
        if (!TweenUtils.typeOP.ContainsKey(typeof(T)))
            TweenUtils.typeOP.Add(typeof(T), new TweenOperator<T>(sub, add, ratio));
    }
}
public abstract class TweenOperator{
    public abstract T Sub<T>(T x, T y);
    public abstract T Add<T>(T x, T y);
    public abstract T Ratio<T>(T x, float y);
}
public class TweenOperator<T> : TweenOperator{
    Func<T, T, T> sub;
    Func<T, T, T> add;
    Func<T, float, T> ratio;
    public TweenOperator(Func<T, T, T> sub, Func<T, T, T> add, Func<T, float, T> ratio)
        => (this.sub, this.add, this.ratio) = (sub, add, ratio);

    public override T1 Add<T1>(T1 x, T1 y){
        if (typeof(T1) != typeof(T))
            throw new InvalidOperatorException("", typeof(T1));
        ref var xx = ref Unsafe.As<T1, T>(ref x);
        ref var yy = ref Unsafe.As<T1, T>(ref y);
        var zz = add(xx, yy);
        return Unsafe.As<T, T1>(ref zz);
    }
    public override T1 Ratio<T1>(T1 x, float y)
    {
        if (typeof(T1) != typeof(T))
            throw new InvalidOperatorException("", typeof(T1));
        ref var xx = ref Unsafe.As<T1, T>(ref x);
        var zz = ratio(xx, y);
        return Unsafe.As<T, T1>(ref zz);
    }

    public override T1 Sub<T1>(T1 x, T1 y)
    {
        if (typeof(T1) != typeof(T))
            throw new InvalidOperatorException("", typeof(T1));
        ref var xx = ref Unsafe.As<T1, T>(ref x);
        ref var yy = ref Unsafe.As<T1, T>(ref y);
        var zz = sub(xx, yy);
        return Unsafe.As<T, T1>(ref zz);
    }
}
