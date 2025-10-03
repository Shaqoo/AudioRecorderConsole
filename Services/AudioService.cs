using AudioProj.Utils;
using NAudio.Wave;

namespace AudioProj.Services
{
   public class AudioService
    {
        public byte[] RecordAudio()
        {
            using var recordedAudio = new MemoryStream();

            using var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 1)
            };

            using var writer = new WaveFileWriter(new IgnoreDisposeStream(recordedAudio), waveIn.WaveFormat);

            waveIn.DataAvailable += (s, e) =>
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            Console.WriteLine("Recording... Press any key to stop.");
            waveIn.StartRecording();
            Console.ReadKey();
            waveIn.StopRecording();

            writer.Flush();

            Console.WriteLine("Recording complete.");

            return recordedAudio.ToArray();
        }

        public void PlayAudio(byte[] audioData)
        {
            using var ms = new MemoryStream(audioData);
            using var reader = new WaveFileReader(ms);
            using var outputDevice = new WaveOutEvent();

            outputDevice.Init(reader);
            outputDevice.Play();

            Console.WriteLine("Playing audio...");

            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine("Playback finished.");
        }
    }
}