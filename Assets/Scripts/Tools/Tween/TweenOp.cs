using System;
using UnityEngine;
using static Tween;
public static class TweenOp{
    public static TweenTrack AddTweenValue<T>(this Tween tween, string trackName, Action<T> setter, T start, T end, float time,
            TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        var tweenDict = tween.tweenDict;
        if (!tweenDict.TryGetValue(trackName, out var track))
        {
            track = new TweenTrack { name = trackName };
            tweenDict.Add(trackName, track);
        }

        return track.AddTweenValue(setter, start, end, time, transitionType, easeType);
    }
    public static TweenTrack AddTweenValue<T>(this Tween tween,
                                              Action<T> setter,
                                              T start,
                                              T end,
                                              float time,
                                              TransitionType transitionType = TransitionType.LINEAR,
                                              EaseType easeType = EaseType.IN)
        => tween.AddTweenValue<T>("__default", setter, start, end, time, transitionType, easeType);
    public static TweenTrack AddTweenGetter<T>(this Tween tween,string trackName, Action<T> setter, Func<T> getter, T end, float time,
           TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        var tweenDict = tween.tweenDict;
        if (!tweenDict.TryGetValue(trackName, out var track))
        {
            track = new TweenTrack { name = trackName };
            tweenDict.Add(trackName, track);
        }
        return track.AddTweenGetter(setter, getter, end, time, transitionType, easeType);
    }

    public static TweenTrack AddTweenGetter<T>(this Tween tween, Action<T> setter, Func<T> getter, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenGetter<T>("__default", setter, getter, end, time, transitionType, easeType);
    public static TweenTrack AddTweenOffset<T>(this Tween tween,string trackName, Action<T> setter, Func<T> getter, T offset, float time,
       TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        var tweenDict = tween.tweenDict;
        if (!tweenDict.TryGetValue(trackName, out var track))
        {
            track = new TweenTrack { name = trackName };
            tweenDict.Add(trackName, track);
        }
        
        return track.AddTweenOffset(setter,getter,offset,time,transitionType,easeType );
    }

    public static TweenTrack AddTweenOffset<T>(this Tween tween,Action<T> setter, Func<T> getter, T end, float time,
    TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenOffset("__default", setter, getter, end, time, transitionType, easeType);

    public static TweenTrack AddTween<T>(this Tween tween,
        Action<T> setter, T start, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenValue<T>(setter, start, end, time, transitionType, easeType);
    public static TweenTrack AddTween<T>(this Tween tween,
        Action<T> setter, Func<T> getter, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenGetter<T>(setter, getter, end, time, transitionType, easeType);
    public static TweenTrack AddTween<T>(this Tween tween, string trackName,
        Action<T> setter, T start, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenValue<T>(trackName, setter, start, end, time, transitionType, easeType);
    public static TweenTrack AddTween<T>(this Tween tween, string trackName,
        Action<T> setter, Func<T> getter, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => tween.AddTweenGetter<T>(trackName, setter, getter, end, time, transitionType, easeType);
}

public static class TweenTrackOp
{
    public static TweenTrack AddTweenValue<T>(this TweenTrack track, Action<T> setter, T start, T end, float time,
            TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        if (track._tweenState == Tween.TweenState.RUNNING)
        {
            Debug.LogError("Try to call AddTween while tween is running");
            return track;
        }

        var cTime = 0f;
        var tweenNodeList = track.tweenNodeList;
        if (tweenNodeList.Count > 0)
            cTime = tweenNodeList[tweenNodeList.Count - 1].time;

        var tweenNode = new TweenValueNode<T>(setter, start, end, time + cTime);
        tweenNode.easeType = easeType;
        tweenNode.transitionType = transitionType;
        tweenNode.master = track;
        tweenNodeList.Add(tweenNode);
        return track;
    }

    public static TweenTrack AddTweenGetter<T>(this TweenTrack track, Action<T> setter, Func<T> getter, T end, float time,
           TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        if (track._tweenState == Tween.TweenState.RUNNING)
        {
            Debug.LogError("Try to call AddTween while tween is running");
            return track;
        }

        var tweenNodeList = track.tweenNodeList;
        var cTime = 0f;
        if (tweenNodeList.Count > 0)
            cTime = tweenNodeList[tweenNodeList.Count - 1].time;

        var tweenNode = new TweenGetterNode<T>(setter, getter, end, time + cTime);
        tweenNode.easeType = easeType;
        tweenNode.transitionType = transitionType;
        tweenNode.master = track;
        tweenNodeList.Add(tweenNode);
        return track;
    }

    public static TweenTrack AddTweenOffset<T>(this TweenTrack track, Action<T> setter, Func<T> getter, T offset, float time,
       TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
    {
        if (track._tweenState == Tween.TweenState.RUNNING)
        {
            Debug.LogError("Try to call AddTween while tween is running");
            return track;
        }

        var cTime = 0f;
        var tweenNodeList = track.tweenNodeList;
        if (tweenNodeList.Count > 0)
            cTime = tweenNodeList[tweenNodeList.Count - 1].time;

        var tweenNode = new TweenOffsetNode<T>(setter, getter, offset, time + cTime);
        tweenNode.easeType = easeType;
        tweenNode.transitionType = transitionType;
        tweenNode.master = track;
        tweenNodeList.Add(tweenNode);
        return track;
    }

    public static TweenTrack AddTween<T>(this TweenTrack track,
        Action<T> setter, T start, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => track.AddTweenValue<T>(setter, start, end, time, transitionType, easeType);
    public static TweenTrack AddTween<T>(this TweenTrack track,
        Action<T> setter, Func<T> getter, T end, float time,
        TransitionType transitionType = TransitionType.LINEAR, EaseType easeType = EaseType.IN)
        => track.AddTweenGetter<T>(setter, getter, end, time, transitionType, easeType);
}


