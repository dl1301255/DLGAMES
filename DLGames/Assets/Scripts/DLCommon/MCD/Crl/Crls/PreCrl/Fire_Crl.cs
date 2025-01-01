using DL.Common;
using DL.DLGame;
using DL.MCD.Data;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DL.MCD
{
    /// <summary>
    /// 管理开火
    /// </summary>
    public class Fire_Crl : MCDCrlSO
    {
        [ShowInInspector]
        private string UPCName = "UnitData";
        [ShowInInspector]
        private string BulletPrefabUpTag = "BulletPrefab";
        [ShowInInspector]
        private string FirePointName;
        [ShowInInspector]
        private string AudioFireName;
        [ShowInInspector]
        private string ConditionName = "FireCondition";
        private string _conditionStr = "Condition";
        public FireData CrFireData = new FireData();

        [ShowInInspector]
        public List<string> DeffConditions;
        [ShowInInspector]
        public Vector3 DeffFirePoint;

        [ShowInInspector]
        private MCDMgr mgr;
        [ShowInInspector]
        private GameObject bulletPrefab;
        [ShowInInspector]
        private GameObject muzzlePrefab;
        [ShowInInspector]
        private AudioClip ac;
        [ShowInInspector]
        private Vector3 firePoint;
        [ShowInInspector]
        private List<string> cacheConditions;
        [ShowInInspector]
        private Dictionary<string, List<IEff>> DicEffsCache;
        private List<IEff> crListIeff;

        public override void Init()
        {
            cacheConditions = new List<string>();
            DicEffsCache = new Dictionary<string, List<IEff>>();
        }


        /// <summary>
        /// 标准shoot 使用mgr自己进行检索
        /// </summary>
        /// <param name="mgr"></param>
        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            this.mgr = mgr;

            if (string.IsNullOrEmpty(BulletPrefabUpTag) || string.IsNullOrEmpty(UPCName)) return;

            if (!getBulletPrefab(out bulletPrefab)) return;

            //condition
            cacheConditions = getConditions(mgr);
            for (int i = 0; i < cacheConditions.Count; i++)
            {
                var sb = GameTool.GetStr("DL.Common.", cacheConditions[i], _conditionStr);
                var v = GameCommonFactory.Instance.CreateMethod<ICondition>(sb.ToString())?.GetBool(this.mgr);

                if (v == false)
                {
                    Reset();
                    return;
                }
            }

            getFirePoint(mgr);
            getMuzzle();
            getAc();

            Fire();
        }

        private List<string> getConditions(MCDMgr mgr)
        {
            var cs = mgr.GetUPC(UPCName)?.GetUP(ConditionName)?.GetSubType<UnitPropertyListStr>().Value;

            if (cs == null || cs.Count <= 0) /*conditions = DeffConditions*/
            {
                cs = DeffConditions;
            }

            return cs;
        }
        private bool getBulletPrefab(out GameObject bullet)
        {
            var tempBulletName = mgr.GetUPC(UPCName)?.GetUP(BulletPrefabUpTag)?.StringVal;

            if (tempBulletName == null)
            {
                bullet = null;
                return false;
            }
            bullet = AssetsManager. Instance.LoadAsset<GameObject>(tempBulletName);
            if (!bullet) return false;
            return true;
        }
        private void getFirePoint(MCDMgr mgr)
        {
            if (string.IsNullOrEmpty(FirePointName))
            {
                firePoint = DeffFirePoint;
            }
            else
            {
                var v = mgr.GetUPC(UPCName)?.GetUP(FirePointName)?.GetSubType<UnitPropertyVector2>().V2;
                Vector3 v3 = new Vector3(v.Value.x, 0, v.Value.y);
                firePoint = v3;
            }
        }
        private void getMuzzle()
        {
            if (!AssetsManager. Instance.TryLoadAsset(GameTool.GetStr(bulletPrefab.name, "_Muzzle"), out muzzlePrefab)) return;
        }
        private void getAc()
        {
            if (string.IsNullOrEmpty(AudioFireName)) return;

            var tempAc = mgr.GetUPC(UPCName)?.GetUP(AudioFireName)?.StringVal;
            if (tempAc == null) return;

            ac = AssetsManager. Instance.LoadAsset<AudioClip>(tempAc, true);
            if (ac == null) return;

        }

        /// <summary>
        /// 涵盖equipment data so 数值 进行shoot 无缓存 纯工厂模式
        ///             1：data SO 作为预设 编辑器中测试用
        ///             2：bulletprefab、ac、muzzle、FirePointV3 手动设定4个点 进行shoot
        /// </summary>
        public override void ExecuteCrl(MCDMgr mgr = null, params object[] ps)
        {
            if (!mgr) return;
            this.mgr = mgr;
            //做一个out 获取bool
            //condition list 直接确认是否可用
            bulletPrefab = (GameObject)ps[0];
            muzzlePrefab = (GameObject)ps[1];
            ac = (AudioClip)ps[2];
            firePoint = (Vector3)ps[3];

            //加入一个detonate eff list，物体在引爆时 会自动加入 eff list 并执行 。
            Fire();
        }

        private void exeListIeff(string tag)
        {
            crListIeff = null;

            if (!DicEffsCache.TryGetValue(tag, out crListIeff))
            {
                crListIeff = new List<IEff>();

                var v = GameTool.GetStringToArray(tag);

                for (int i = 0; i < v.Length; i++)
                {
                    var s = GameTool.GetStr("DL.Common.", v[i], "Eff");
                    if (string.IsNullOrEmpty(s)) continue;
                    var eff = GameCommonFactory.Instance.CreateMethod<IEff>(s);
                    if (eff == null) continue;
                    crListIeff.Add(eff);
                }

                DicEffsCache.Add(tag, crListIeff);
            }

            if (crListIeff == null || crListIeff.Count <= 0)
            {
                Debug.Log("CrListIeff == null ");
                return;
            }

            for (int i = 0; i < crListIeff.Count; i++)
            {
                crListIeff[i].Execute(CrFireData);
            }

            return;
        }

        [Button]
        public virtual void Fire()
        {
            if (!bulletPrefab || !mgr) return;

            EventManager.Instance.SendEvent("StartFire", mgr);

            //火光
            if (muzzlePrefab != null)
            {
                var m = GameObjectPool.Instance.CreateObject(muzzlePrefab.name, muzzlePrefab, mgr.transform.position + firePoint, mgr.transform.rotation);
                GameObjectPool.Instance.CollectObject(m, 0.5f);
            }


            //声音
            if (ac)
            {
                MMSoundManagerSoundPlayEvent.Trigger(ac, MMSoundManager.MMSoundManagerTracks.Sfx, mgr.transform.position + firePoint);
            }

            //子弹
            var go = GameObjectPool.Instance.CreateObject(bulletPrefab.name, bulletPrefab, mgr.transform.position + firePoint, mgr.transform.rotation, null, InitBullet);

            //播放效果列表

            EventManager.Instance.SendEvent("Fired", CrFireData);

            setCd();
            Reset();
        }

        private void setCd()
        {
            var cd = mgr.GetUPC(UPCName)?.GetUP("CD");
            if (cd == null)
            {
                Debug.Log("cd = 0");
            }
            var cddel = mgr.GetUPC(UPCName)?.GetUP("CD_Del")?.GetSubType<UnitPropertyFloat>();
            if (cddel == null) return;
            cddel.Value = float.Parse(cd.stringVal);
            cddel.stringVal = cd.stringVal;
        }

        private void Reset()
        {
            mgr = null;
            bulletPrefab = null;
            muzzlePrefab = null;
            ac = null;
            CrFireData.Clear();
        }

        private void InitBullet(GameObject go)
        {
            if (!go) return;

            //go.GetComponent<MCDMgr>()?.ListUPC.Add(mgr.GetUPC(UPCName));
            //赋值：atk eff owermgrID 修改tag layer
            var goMgr = go.GetComponent<MCDMgr>();
            if (!goMgr) return;
            goMgr.Init();

            CrFireData.BulletMgr = go.GetComponent<Bullet_DL>();
            CrFireData.FirerMgr = mgr;
            CrFireData.BulletMgr.ListUPC.Add(mgr.ListUPC[0]);

        }
        private void SetOwerMgrValue(MCDMgr goMgr)
        {
            var v = goMgr.GetUP("UnitData", "OwerMgrID");
            if (v == null) return;
            v.stringVal = mgr.GetUPC("UnitData")?.UPCName;
        }

        private void SetTagAndLayer(GameObject go)
        {
            if (mgr.gameObject.tag == "Player")
            {
                if (go.tag != "Player") go.tag = "Player";
                if (go.layer != 11) go.layer = 11;
            }
            else if (mgr.tag == "Enemy")
            {
                if (go.tag != "Enemy") go.tag = "Enemy";
                if (go.layer != 12) go.layer = 12;
            }
        }

        private void SetAtkValue(MCDMgr goMgr, MCDMgr owerMgr)
        {
            var v = goMgr.GetUP("UnitData", "Atk")?.GetSubType<UnitPropertyFloat>();
            var v1 = owerMgr.GetUP("UnitData", "Atk")?.GetSubType<UnitPropertyFloat>();
            if (v == null || v1 == null) return;
            v.Value = v1.Value;
        }

        private void SetBDEffsValue(MCDMgr goMgr, MCDMgr owerMgr)
        {
            var v = goMgr.GetUP("UnitData", "BDEffs")?.GetSubType<UnitPropertyListStr>();
            var v1 = owerMgr.GetUP("UnitData", "BDEffs")?.GetSubType<UnitPropertyListStr>();
            if (v == null || v1 == null) return;
            v1.Value.ForEach(s => v.Value.Add(s));
        }
    }
    public class FireData
    {
        public MCDMgr FirerMgr;
        public MCDMgr BulletMgr;
        public List<MCDMgr> ListBulletMgr;

        public void Clear()
        {
            if (FirerMgr != null) FirerMgr = null;
            if (BulletMgr != null) BulletMgr = null;
        }
    }
}
