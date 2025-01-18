using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using UnityEngine;
using UnityEngine.Analytics;

using TypeCallBack = System.Action<EventStream.Event, System.Collections.Generic.List<string>>;
class EventStream : MonoSingleton<EventStream>
{
    public class Task
    {
        static Dictionary<TaskType, TypeCallBack> taskCallback 
            = new Dictionary<TaskType, TypeCallBack>()
        {
            {TaskType.Error,null }
        };
        public enum TaskType{
            Error,
        }
        public TaskType type;
        List<string> args = new List<string>();
        public void Invoke(Event who) {
            var callback = taskCallback[type];
            callback.Invoke(who, args);
        }
        public static TaskType ParseTaskType(string str){
            switch(str){
                default:
                    return TaskType.Error;
            }
        }
        public static Task Parse(string str){
            var parts = str.Split(":");
            var name = parts[0];
            var args = string.Join(":", parts.Skip(1)).Split('|').ToList();
            return new Task() {type = ParseTaskType(name), args = args};   
        }

    }
    public class Condition{
        public enum Method{
            Error,
            LT,
            LE,
            EQ,
            GE,
            GT
        }
        public EventStream master;
        public string varName;
        public Method method;
        public float target;

        public bool Valid {
            get{
                switch(method){
                    case Method.LT:
                        return target < master.globalVariable[varName];
                    case Method.LE:
                        return target <= master.globalVariable[varName];
                    case Method.EQ:
                        return target == master.globalVariable[varName];
                    case Method.GE:
                        return target >= master.globalVariable[varName];
                    case Method.GT:
                        return target > master.globalVariable[varName];
                }
                return false;
            }
        }
        public static Condition Parse(string str){
            var parts = str.Split(":");
            var name = parts[0];
            var method = parts[1].Split('|')[0];
            var target = float.Parse(parts[1].Split('|')[1]);
            var condition = new Condition(){varName = name, target = target};
            if (method == "LT") condition.method = Method.LT;
            else if (method == "LE") condition.method = Method.LE;
            else if (method == "EQ") condition.method = Method.EQ;
            else if (method == "GE") condition.method = Method.GE;
            else if (method == "GT") condition.method = Method.GT;
            return condition;
        }
    }
    public class Trigger 
    {
        public enum TriggerType{
            Error,
            EventStatus,
        }
        public TriggerType triggerType;
        public List<string> args = new List<string>();
        public static TriggerType ParseType(string str){
            if (str == "event") return TriggerType.EventStatus;
            return TriggerType.Error;
        }
        public static Trigger Parse(string str){
            var parts = str.Split(":");
            var name = parts[0];
            var args = string.Join(":", parts.Skip(1)).Split('|').ToList();
            return new Trigger() { triggerType = ParseType(name), args = args };   
        }
        public virtual bool Equals(Trigger x)
        {
            return args == ((Trigger)x).args;     
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(triggerType, args);
        }
    }
    public class Event 
    {
        public int id;
        public EventStream master;
        public List<Task> running = new List<Task>();
        public List<Task> onEventBegin = new List<Task>();
        public List<Task> onEventEnd = new List<Task>();

        public List<Condition> conditions = new List<Condition>();
        public Dictionary<Trigger, int> inDegree = new Dictionary<Trigger, int>();
        public Dictionary<Task, int> taskFinish = new Dictionary<Task, int>();
        public bool TriggerEvent(Trigger trigger){
            inDegree[trigger]++;
            foreach (var c in conditions)
                if (!c.Valid)
                    return false;
            foreach(var p in inDegree){
                if (p.Value == 0)
                    return false;
            }
            return true;
        }

        public void EventBegin(){
            if(onEventBegin != null)
                for (int i = 0; i < onEventBegin.Count; i++)
                    onEventBegin[i].Invoke(this);
            var trigger = new Trigger {
                triggerType = Trigger.TriggerType.EventStatus,
                args = new List<string>(){"begin"}
            };
            master.TriggerEvent(trigger);
        }
        public bool TaskFinish(Task who){
            taskFinish[who]++;
            bool allFinish = true;
            foreach (var task in taskFinish)
                if (task.Value == 0){
                    allFinish = false;
                    break;
                }
            EventEnd();
            return allFinish;
        }
        public void EventEnd(){
            var trigger = new Trigger {
                triggerType = Trigger.TriggerType.EventStatus,
                args = new List<string>(){"end"}
            };
            master.TriggerEvent(trigger);
            if(onEventEnd != null)
                for (int i = 0; i < onEventEnd.Count; i++)
                    onEventEnd[i].Invoke(this);             
        }
    }

    Dictionary<int, Event> events = new Dictionary<int, Event>();
    Dictionary<Trigger, List<int>> triggerEvents = new Dictionary<Trigger, List<int>>();
    Dictionary<int, int> triggeredTimes = new Dictionary<int, int>();
    Dictionary<int, int> maxTriggeredTimes = new Dictionary<int, int>();
    Dictionary<string, float> globalVariable = new Dictionary<string, float>();
    
    void LoadEventFromTable(){
        int cntEventId = -1;
        Action<EventCSVReader.Record> process = (EventCSVReader.Record record) => {
            var s_id = record.data[0];
            var s_inDegree = record.data[1];
            var s_condition = record.data[2];
            var s_onEventBegin = record.data[3];
            var s_onEventEnd = record.data[4];
            var id = int.Parse(s_id);
            Event cntEvent = null;
            if (id != cntEventId) {
                cntEventId = id;
                cntEvent = events[cntEventId] = new Event() { id = cntEventId };
            }
            else cntEvent = events[cntEventId];
            if (s_inDegree.Length != 0)
                cntEvent.inDegree[Trigger.Parse(s_inDegree)]=0;
            if (s_condition.Length != 0)
                cntEvent.conditions.Append(Condition.Parse(s_condition));
            if(s_onEventBegin.Length != 0)
                cntEvent.onEventBegin.Append(Task.Parse(s_onEventBegin));
            if(s_onEventEnd.Length != 0)
                cntEvent.onEventEnd.Append(Task.Parse(s_onEventEnd));

        };
        EventCSVReader.ReadFromFile("event.csv",process);
    }
    void TriggerEvent(Trigger trigger){
        if(!triggerEvents.TryGetValue(trigger, out var eventList)){
            Debug.LogWarning("EventStream: triggered some unknown trigger");
            return;
        }
        for(int i = 0;i < eventList.Count;i++){
            var e = events[eventList[i]];
            if (triggeredTimes[e.id] > maxTriggeredTimes[e.id])
                return;
            bool success = e.TriggerEvent(trigger); 
            if(success){
                triggeredTimes[e.id]++;
                e.EventBegin();
            }
        }
    }
}