using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de Audio")]
    private AudioSource fuenteMusica;
    private AudioSource fuenteSFX;

    [Header("Música de Fondo")]
    public AudioClip musicaFondo;
    public AudioClip musicaPowerUp; // <--- Música especial de la Lluvia Dorada

    [Header("Efectos de Sonido (SFX)")]
    public AudioClip sonidoObjetoBueno;
    public AudioClip sonidoObjetoMalo;
    public AudioClip sonidoPowerUp;
    public AudioClip sonidoVictoria;
    public AudioClip sonidoDerrota;

    private Coroutine corrutinaMusicaPowerUp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InicializarFuentesAudio();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += AlCargarEscena;
    private void OnDisable() => SceneManager.sceneLoaded -= AlCargarEscena;

    void Start()
    {
        ReproducirMusicaFondo();
    }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (fuenteMusica != null && !fuenteMusica.isPlaying)
        {
            ReproducirMusicaFondo();
        }
    }

    private void InicializarFuentesAudio()
    {
        fuenteMusica = gameObject.AddComponent<AudioSource>();
        fuenteMusica.loop = true;
        fuenteMusica.playOnAwake = false;
        fuenteMusica.volume = 0.4f;

        fuenteSFX = gameObject.AddComponent<AudioSource>();
        fuenteSFX.loop = false;
        fuenteSFX.playOnAwake = false;
        fuenteSFX.volume = 1.0f;
    }

    public void ReproducirMusicaFondo()
    {
        if (musicaFondo != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaFondo;
            fuenteMusica.Play();
        }
    }

    // Cambia la música a la del Power-Up durante los segundos especificados
    public void ActivarMusicaPowerUp(float duracion)
    {
        if (corrutinaMusicaPowerUp != null)
            StopCoroutine(corrutinaMusicaPowerUp);

        corrutinaMusicaPowerUp = StartCoroutine(RutinaMusicaPowerUp(duracion));
    }

    private IEnumerator RutinaMusicaPowerUp(float duracion)
    {
        if (musicaPowerUp != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaPowerUp;
            fuenteMusica.Play();
        }

        yield return new WaitForSeconds(duracion);

        // Al terminar el tiempo, regresa a la música normal del nivel
        ReproducirMusicaFondo();
    }

    public void DetenerMusica()
    {
        if (corrutinaMusicaPowerUp != null) StopCoroutine(corrutinaMusicaPowerUp);
        if (fuenteMusica != null) fuenteMusica.Stop();
    }

    public void PlayObjetoBueno() => ReproducirSFX(sonidoObjetoBueno);
    public void PlayObjetoMalo() => ReproducirSFX(sonidoObjetoMalo);
    public void PlayPowerUp() => ReproducirSFX(sonidoPowerUp);

    public void PlayVictoria()
    {
        DetenerMusica();
        ReproducirSFX(sonidoVictoria);
    }

    public void PlayDerrota()
    {
        DetenerMusica();
        ReproducirSFX(sonidoDerrota);
    }

    private void ReproducirSFX(AudioClip clip)
    {
        if (clip != null && fuenteSFX != null)
        {
            fuenteSFX.PlayOneShot(clip);
        }
    }
}