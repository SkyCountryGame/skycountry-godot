
using System.Collections.Generic;

//Event System: each event just has a type and a payload. listeners know how to handle the events they listen for and what type of payload to expect.
//we could make an EventPayload abstract class if needed. or define specific Event classes (e.g. QuestReceivedEvent extends Event) with members for its specific payload

public enum EventType {
    GameOver, QuestReceived, QuestComplete, QuestFailed, Death, //just some ideas for now. would be a good to define payload in comment of each event type. and probably its intended use
    WeatherChange, DialogueInitiated, EntityAttacked, EntityDamaged,
    SpawnParticles, //payload = Vector3 (location)
    PlaySound, //payload = Tuple<string, int> (sound file path, volume)
    SetMusic, 
    CustomScene1, //for example some little scene plays out after a dialogue
    BeginTradeUI,
    None //default
}

/**
    an in game event that things can listen for
*/
public class Event {
    public EventType eventType;
    public dynamic payload;
    public Event(EventType t = EventType.None, dynamic p = null){
        eventType = t;
        payload = p;
    }
    public bool Invoke(){
        EventManager.Invoke(this);
        return true;
    }
    //idea to include checking of if event can be invoked within the event itself (in Invoke()). we'll see if the other logic ends up handling this as needed
    //public abstract bool CanInvoke();
}

public interface EventListener {
    HashSet<EventType> eventTypes { get; } //what types of event does this thing care about?
    void HandleEvent(Event e);
}

public class EventManager {
    private static Dictionary<EventType, List<EventListener>> eventListeners = new Dictionary<EventType, List<EventListener>>();
    public static void RegisterListener(EventListener listener){
        foreach (EventType t in listener.eventTypes){
            if (!eventListeners.ContainsKey(t)){
                eventListeners[t] = new List<EventListener>(){};
            }
            eventListeners[t].Add(listener);
        }
    }

    public static void Invoke(Event e){
        if (eventListeners.ContainsKey(e.eventType)){
            foreach (EventListener l in eventListeners[e.eventType]){
                l.HandleEvent(e);
            }
        }
    }
    //a convenience method that will construct the event object for you
    public static void Invoke(EventType t, dynamic payload = null){
        Event e = new Event(t, payload);
        Invoke(e);
    }
}