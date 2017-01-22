using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;              
	public AudioSource musicSource;   
	public AudioSource underwaterMusicSource;   
	public AudioSource underwaterAmbientSource;   
	public static SoundManager instance = null;  

	public AudioClip splash;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		//splash = Resources.Load("splash.mp3") as AudioClip;

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}


	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;

		//Play the clip.
		efxSource.Play ();
	}

	public void GoUnderwater() {
		
		musicSource.DOFade(0.0f, 2.0f);

		underwaterAmbientSource.mute = false;
		underwaterAmbientSource.Play ();
		underwaterAmbientSource.DOFade (1.0f, 10.0f);

		PlaySingle (splash);


		Invoke ("PlayUnderwaterMusic", 4.0f);


		//underwaterMusicSource.Play ();
		//underwaterMusicSource.volume = 0.0f;
		//underwaterMusicSource.DOFade (1.0f, 0.1f).SetDelay(10.0f);




	}

	public void PlayUnderwaterMusic() {
		underwaterMusicSource.mute = false;
		underwaterMusicSource.volume = 1.0f;
		underwaterMusicSource.Play ();
	}
		
}