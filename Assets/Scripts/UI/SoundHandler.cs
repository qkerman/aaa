using System;
using UnityEngine;
using UnityEngine.UI;

namespace EVA
{
    /// <summary>
    /// This class is the controller for the Sound modular interface.
    /// It let the user play/pause, stop, loop or spatialized the sound.
    /// It also let the user change the volume.
    /// </summary>
    public class SoundHandler : ModularHandler
    {
        /// <summary>
        /// The visible value of the sound.
        /// </summary>
        public InputField volume;
        /// <summary>
        /// The play/pause toogle.
        /// </summary>
        public Toggle playPause;
        /// <summary>
        /// The loop toogle.
        /// </summary>
        public Toggle loop;
        /// <summary>
        /// The spatialized toogle.
        /// </summary>
        public Toggle spatialized;
        /// <summary>
        /// The volume slider.
        /// </summary>
        public Slider slide;
        /// <summary>
        /// The link to our class on the handledObject.
        /// </summary>
        private HasSound sound;

        /// <summary>
        /// Called by the slider to change the volume of the <see cref="sound"/> and another part of the view.
        /// </summary>
        /// <param name="newVolume">New value of the volume.</param>
        public void AdjustVolume(Single newVolume)
        {
            sound.Volume = ((int)newVolume);
            volume.text = sound.Volume.ToString();
        }

        /// <summary>
        /// The update method that updates the play/pause toggle state.
        /// </summary>
        private void Update()
        {
            if(handledObject is object && sound is object)
            {
                if (!sound.IsPlaying && handledObject.GetComponent<AudioSource>().time != 0d)
                {
                    playPause.isOn = false;
                }
            }
        }

        /// <summary>
        /// Called by the <see cref="playPause"/> toogle to play or pause the <see cref="sound"/>.
        /// </summary>
        /// <param name="value">Boolean representing if we should play or pause the sound.</param>
        public void PlayPauseSound(bool value)
        {
            if (value)
            {
                sound.Play();
            }
            else
            {
                sound.Pause();
            }
        }
        /// <summary>
        /// Called by the stop Button to stop the <see cref="sound"/>.
        /// </summary>
        public void StopSound()
        {
            sound.Stop();
            playPause.isOn = false;
        }

        /// <summary>
        /// Called by the <see cref="loop"/> toogle to loop or not the <see cref="sound"/>.
        /// </summary>
        /// <param name="value">Boolean representing if we should loop the sound.</param>
        public void LoopingSound(bool value)
        {
            sound.Looping = !value;
        }

        /// <summary>
        /// Called by the <see cref="spatialized"/> toogle to spatialized or not the <see cref="sound"/>.
        /// </summary>
        /// <param name="value">Boolean representing if we should loop the sound.</param>
        public void SpatializedSound(bool value) 
        {
            sound.SpatializedSound = !value;
        }

        /// <summary>
        /// First do the same as the <see cref="ModularHandler"/>, then intialise the view with the value of the parameter.
        /// </summary>
        /// <param name="obj">The object the view is managing.</param>
        public override void setObject(GameObject obj)
        {
            base.setObject(obj);
            Debug.Log(handledObject.GetComponent<HasSound>());
            sound = handledObject.GetComponent<HasSound>();
            Debug.Log(sound.Volume.ToString());
            volume.text = sound.Volume.ToString();
            slide.value = sound.Volume;
            playPause.isOn = sound.IsPlaying;
            loop.isOn = !sound.Looping;
            spatialized.isOn = !sound.SpatializedSound;
        }
    }
}

