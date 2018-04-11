using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Threading;

namespace RockGamer.Gamer
{
    public enum GameSoundType
    {
        Shoot
    }

    public class GameSound
    {
        SoundEffect effect;
        SoundEffectInstance[] instance;
        public SoundEffectInstance loop;

        int i = 0;

        public GameSound(SoundEffect e, int soundChannels)
        {
            instance = new SoundEffectInstance[soundChannels];
            effect = e;
            loop = effect.CreateInstance();
            for(int i = 0; i < instance.Length; i++)
                instance[i] = effect.CreateInstance();
        }

        public void Play()
        {
            instance[i].Volume = SoundManager.Sfx * SoundManager.Master;
            instance[i].Play();
            i++;
            if(i > instance.Length - 1)
                i = 0;
        }

        public void Loop()
        {
            loop.IsLooped = true;
            loop.Play();
        }

        public void Loop(float vol)
        {
            loop.Volume = vol * SoundManager.Master;
            loop.IsLooped = true;
            loop.Play();
        }

    }

    public static class SoundManager
    {

        public static float Sfx = 0.5f;
        public static float Music = 0.5f;
        public static float Master = 0.5f;

        public static int SoundChannels = 16;

        public static Dictionary<GameSoundType, GameSound> Sounds;
        static ContentManager c;

        public static void Load(ContentManager cm)
        {
            c = cm;
            Sounds = new Dictionary<GameSoundType, GameSound>();

        }

        static void AddSound(GameSoundType type, string path)
        {
            var ef = c.Load<SoundEffect>(path);
            var gs = new GameSound(ef, SoundChannels);
            Sounds.Add(type, gs);
        }

        public static void PlaySound(GameSoundType type)
        {
            Sounds[type].Play();
        }

        public static void LoopSound(GameSoundType type)
        {
            Sounds[type].Loop();
        }

        public static void LoopSound(GameSoundType type, float volume)
        {
            Sounds[type].Loop(volume);
        }

        public static void StopLoop(GameSoundType type)
        {
            Sounds[type].loop.Stop();
        }

    }
}
