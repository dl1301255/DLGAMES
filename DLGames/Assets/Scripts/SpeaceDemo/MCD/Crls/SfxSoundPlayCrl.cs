using DL.MCD;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoreMountains.Tools.MMSoundManager;

namespace DL.DLGame
{
    public class SfxSoundPlayCrl : MCDCrlSO
    {

        public AudioClip AudioClip;
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            MMSoundManagerSoundPlayEvent.Trigger(AudioClip, MMSoundManagerTracks.Sfx, mgr.gameObject.transform.position);
        }
    }
}
