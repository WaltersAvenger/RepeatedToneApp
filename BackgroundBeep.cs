using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RepeatedToneApp
{
    class BackgroundBeep
    {
        static Thread _beepThread;
        static AutoResetEvent _signalBeep;
        static SoundPlayer player;

        public BackgroundBeep(Int32 frequency, Int32 amplitude)
        {
            player = new SoundPlayer();
            player.SoundLocation = GenerateWAV(frequency, amplitude);
            _signalBeep = new AutoResetEvent(false);
            _beepThread = new Thread(() =>
            {
                for (; ; )
                {
                    _signalBeep.WaitOne();
                    player.PlayLooping();
                }
            }, 1);
            _beepThread.IsBackground = true;
            _beepThread.Start();
        }

        public void Beep()
        {
            _signalBeep.Set();
        }

        public void StopBeep()
        {
            player.Stop();
        }

        private string GenerateWAV(double naturalFrequency, Int32 amplitude)
        {
            string path = "test.wav";
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            int RIFF = 0x46464952;
            int WAVE = 0x45564157;
            int formatChunkSize = 16;
            int headerSize = 8;
            int format = 0x20746D66;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int data = 0x61746164;
            int samples = 88200 * 4;
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            writer.Write(RIFF);
            writer.Write(fileSize);
            writer.Write(WAVE);
            writer.Write(format);
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(data);
            writer.Write(dataChunkSize);
            double ampl = 2500 * amplitude;
            for (int i = 0; i < samples; i++)
            {
                double t = (double)i / (double)samplesPerSecond;
                short s = (short)(ampl * (Math.Sin(t * naturalFrequency * 2.0 * Math.PI)));
                writer.Write(s);
            }
            writer.Close();
            stream.Close();
            return path;
         }
    }
}
