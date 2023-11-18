using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    [SerializeField] AudiosReferenceSO audiosReferenceSO;
    void Awake()
    {
        if(Instance != null) {Debug.LogError("There is more than 1 soundmanager instance");return;}
        Instance = this;
    }
    private void Start()
    {
        Player.Instance.OnPickSomething += OnPickup;
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

        PlaySound(audiosReferenceSO.objectPickup, Player.Instance.transform.position);
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

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(audiosReferenceSO.footstep, position);
    }
}
