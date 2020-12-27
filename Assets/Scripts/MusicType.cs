using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music Type", menuName = "Music Type")]
public class MusicType : ScriptableObject
{
    public AudioClip[] music = new AudioClip[17];
}
