
namespace Jemeza.SFASFX
{
    using UnityEngine   ;
    using UnityEngine.UI;
    
    public sealed class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private int         _maxIndex  ;
        [SerializeField] private int         _minIndex  ;
        [SerializeField] private Slider      _volume    ;
        [SerializeField] private Text        _clipName  ;              
        [SerializeField] private int         _soundI    ;
        [SerializeField] private AudioSource _playback  ;
        [SerializeField] private AudioClip[] _sounds    ;

        public void Start()
        {
            _volume.onValueChanged.AddListener( OnSliderAdjusted );
            UpdateClip()                                          ;
        }

        public void UpdateClip()
        {
            _playback.clip = _sounds[_soundI]                                   ;
            _playback.Play()                                                    ; 
            _clipName.text = _soundI.ToString() + " : "  + _sounds[_soundI].name;
        }

        public void RestartSound()
        {
            _playback.Stop();
            _playback.Play();
        }

        public void OnSliderAdjusted( float _value )
        {
            _playback.volume = _value;
        }

        public void IncrementSound()
        {
            _soundI++;

            if( _soundI == _maxIndex )
                _soundI =  _minIndex + 1;

            UpdateClip();
        }

        public void DecrementSound()
        {
            _soundI--;

            if (_soundI == _minIndex)
                _soundI = _maxIndex - 1;

            UpdateClip();
        }
    }
}