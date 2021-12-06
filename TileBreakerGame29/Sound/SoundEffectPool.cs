using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;



namespace BreakOut.Sound
{
    class SoundEffectPool
    { 
        private List<SoundEffectInstance> instancePool;        

        public SoundEffectPool(SoundEffect soundEffect, int numberOfInstances)
        {            
            instancePool = new List<SoundEffectInstance>(numberOfInstances);            

            for (int counter = 0; counter < numberOfInstances; counter++)
            {
                instancePool.Add(soundEffect.CreateInstance());
            }
        }
        
        public void Play()
        {            
            foreach (SoundEffectInstance si in instancePool)
            {
                if (si.State == SoundState.Stopped)
                {
                    try
                    {                        
                        si.IsLooped = false;
                        si.Volume = SoundManager.GetEffectVolume();
                        si.Play();
                        
                    }
                    catch (InstancePlayLimitException)
                    {
                        
                    }
                    
                    break;
                }                
            }            
        }

        public SoundEffectInstance PlaySongLoop()
        {
            foreach (SoundEffectInstance si in instancePool)
            {
                if (si.State == SoundState.Stopped)
                {
                    try
                    {
                        si.IsLooped = true;
                        si.Play();
                        return si;
                    }
                    catch (InstancePlayLimitException)
                    {
                        return null;
                    }                    
                }
            }
            return null;
        }
    }
}