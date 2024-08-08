using Godot;

public class AtmosphereManager {
    private AudioStreamPlayer3D audPlayer;

    public AtmosphereManager(){
        audPlayer = new AudioStreamPlayer3D();
        //http stream 
        //HTTPStream httpStream = new HTTPStream();
        //httpStream.SetDownloadLocation("http://example.com/audio.ogg"); // Replace with your audio URL

        //AudioStreamOGGVorbis oggVorbisStream = new AudioStreamOGGVorbis();
        //oggVorbisStream.Stream = httpStream;

        //audPlayer.Stream = oggVorbisStream;
        //audPlayer.Play();
    }
}