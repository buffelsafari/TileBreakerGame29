using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

namespace BreakOut.Sound
{
    enum SoundId : int
    {
        musicLoop1,
        ballToWall,
        ballToBall,
        ballToBat,
        bigExplosion,
        blastershoot,
        blasterhit,
        tilebreak,
        tilehit,
        extender,
        denial,
        bonus,
        extralife,
        tap,
        lid,
        flush,
    }
    
    static class SoundManager
    {      
        static private Dictionary<SoundId, SoundEffectPool> sounds= new Dictionary<SoundId, SoundEffectPool>();
        static private float songVolume;
        static private float effectVolume;
        static private float masterVolume;
        static public SoundEffectInstance song;

        public static bool AddSound(SoundId id, SoundEffect soundEffect, int numberOfInstances)
        {
            if(sounds.TryAdd(id, new SoundEffectPool(soundEffect, numberOfInstances)))
            {
                return true;
            }
            return false;
        }

        public static void SetMasterVolume(float volume)
        {
            masterVolume = volume;
            SoundEffect.MasterVolume = volume;
        }

        public static float GetMasterVolume()
        {
            return masterVolume;
        }

        public static float GetEffectVolume()
        {
            return effectVolume;
        }

        public static void SetEffectVolume(float volume)
        {
            effectVolume = volume;
        }

        public static float GetSongVolume()
        {
            return songVolume;
        }

        public static void Play(SoundId id)
        {
            SoundEffectPool sp;
            if (sounds.TryGetValue(id, out sp))
            {
                sp.Play();                
            }            
        }

        public static SoundEffectInstance PlaySongLoop(SoundId id)
        {
            SoundEffectPool sp;
            if (sounds.TryGetValue(id, out sp))
            {                
                SoundEffectInstance sei= sp.PlaySongLoop();
                sei.Volume = songVolume;                
                return sei;
            }
            return null;
        }

        public static void PauseSongLoop(SoundEffectInstance soundEffectInstance)
        {
            soundEffectInstance.Pause();
        }

        public static void ResumeSongLoop(SoundEffectInstance soundEffectInstance)
        {
            soundEffectInstance.Volume = songVolume;
            soundEffectInstance.Resume();
        }

        public static void SetSongVolume(float volume)
        {
            songVolume = volume;

            if (song != null)
            {
                song.Volume = songVolume;
            }
        }
    }
}