namespace EVA
{
    /// <summary>
    /// This interface brings methods to scripts that manages artworks containing sounds.
    /// </summary>
    public interface HasSound
    {
        /// <summary>
        /// This property inform if the sound is actually playing.
        /// </summary>
        public abstract bool IsPlaying
        {
            get;
        }
        /// <summary>
        /// This property manages spatialization of the sound, which is a setting of the AudioSource component.
        /// </summary>
        public abstract bool SpatializedSound
        {
            get;
            set;
        }

        /// <summary>
        /// This property manages looping of the sound or video, which is a setting of the AudioSource and VideoPlayer component, depending on what is used in the given class/prefab.
        /// </summary>
        public abstract bool Looping
        {
            get;
            set;
        }

        /// <summary>
        /// This property manages the volume of the sound, which is a setting of the AudioSource component.
        /// </summary>
        /// <remarks>
        /// A conversion is done between the setting of the component (which is a float between 0f and 1f) and the volume taken and given by the getter and setter (which is an integer between 0 and 100).
        /// </remarks>
        public abstract int Volume
        {
            get;
            set;
        }

        /// <summary>
        /// This function plays the sound from needed component (AudioSource and possibly VideoPlayer).
        /// </summary>
        public abstract void Play();
        /// <summary>
        /// This function pauses the sound from needed component (AudioSource and possibly VideoPlayer).
        /// </summary>
        public abstract void Pause();
        /// <summary>
        /// This function stops the sound from needed component (AudioSource and possibly VideoPlayer).
        /// </summary>
        public abstract void Stop();
    }
}
