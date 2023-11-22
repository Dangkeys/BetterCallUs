using System;

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    const string  PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance {get; private set;}
    [SerializeField] AudiosReferenceSO audiosReferenceSO;
    float volume = 1f;
    void Awake()
    {
        if(Instance != null) {Debug.LogError("There is more than 1 soundmanager instance");return;}
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        Player.OnAnyPickedSomething += OnPickup;
        DeliveryManager.Instance.OnRecipeSuccess += OnDeliverySuccess;
        DeliveryManager.Instance.OnRecipeFailed += OnDeliveryFailed;
        CuttingCounter.OnAnyCut += OnAnyCut;
        BaseCounter.OnAnyObjectPlacedHere += OnDropItem;
        TrashCounter.OnAnyObjectTrashed += OnTrashed;
    }

    private void OnTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audiosReferenceSO.trash, trashCounter.transform.position);
    }

    private void OnDropItem(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audiosReferenceSO.objectDrop, baseCounter.transform.position);
    }

    private void OnPickup(object sender, EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audiosReferenceSO.objectPickup, player.transform.position);
    }

    private void OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audiosReferenceSO.chop, cuttingCounter.transform.position);
    }

    private void OnDeliveryFailed(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
       PlaySound(audiosReferenceSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void OnDeliverySuccess(object sender, EventArgs e)
    {
         DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audiosReferenceSO.deliverySuccess, deliveryCounter.transform.position);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(audiosReferenceSO.footstep, position);
    }
    public void PlayCountdownSound()
    {
        PlaySound(audiosReferenceSO.warning, Vector3.zero);
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audiosReferenceSO.warning, position);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if(volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
