using System.Collections.Generic;
using Godot;

public class AtmosphereManager : EventListener {
    private AudioStreamPlayer3D audPlayer;

    public AtmosphereManager(){
        EventManager.RegisterListener(this);
        audPlayer = new AudioStreamPlayer3D();
        //http stream 
        //HTTPStream httpStream = new HTTPStream();
        //httpStream.SetDownloadLocation("http://example.com/audio.ogg"); // Replace with your audio URL

        //AudioStreamOGGVorbis oggVorbisStream = new AudioStreamOGGVorbis();
        //oggVorbisStream.Stream = httpStream;

        //audPlayer.Stream = oggVorbisStream;
        //audPlayer.Play();
    }

    public HashSet<EventType> eventTypes => new HashSet<EventType>(){EventType.WeatherChange, EventType.SpawnParticles, EventType.PlaySound};

    public void HandleEvent(Event e)
    {
        GD.Print($"AtmosphereManager handling event {e.eventType.ToString()}");
        switch (e.eventType){
            case EventType.WeatherChange:
                //change the weather
                break;
            case EventType.SpawnParticles:
                if (e.payload is Vector3){
                    Vector3 loc = e.payload; //TODO
                }
                break;
            case EventType.PlaySound:
                
                break;
        }
    }

}