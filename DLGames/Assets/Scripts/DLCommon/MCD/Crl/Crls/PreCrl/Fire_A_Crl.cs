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
    public class Fire_A_Crl : MCDCrlSO
    {
        [ShowInInspector]
        private string UPCName = "UnitData";
        [ShowInInspector]
        private string BulletPrefabUpTag = "BulletPrefab";
        [ShowInInspector]
        private string FirePointName;
        [ShowInInspector]
        private string ConditionName = "FireCondition";
        private string _conditionStr = "Condition";
        public FireData CrFireData = new FireData();

        public List<string> DeffConditions;
        public Vector3 DeffFirePoint;

        private MCDMgr mgr;
        private GameObject bulletPrefab;
        private GameObject muzzlePrefab;
        private Vector3 firePoint;
        private List<string> cacheConditions;

        [ShowInInspector]
        private Dictionary<string, List<IEff>> DicEffsCache;
        private List<IEff> crListIeff;

        public override void Init()
        {
            DicEffsCache = new Dictionary<string, List<IEff>>();
        }

        public override void Cloes()
        {
            base.Cloes();
            DicEffsCache.Clear();
        }

        public override void ExecuteCrl(MCDMgr mgr = null)
        {
            if (mgr == null) return;
            this.mgr = mgr;

            if (string.IsNullOrEmpty(BulletPrefabUpTag) || string.IsNullOrEmpty(UPCName)) return;

            if (!getBulletPrefab(out bulletPrefab)) return;

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
            bullet = AssetsManager.Instance.LoadAsset<GameObject>(tempBulletName);
            if (!bullet) return false;
            return true;
        }
        private void getFirePoint(MCDMgr mgr)
        {
            if (string.IsNullOrEmpty(FirePointName))
            {
                
            }
            else
            {
                var v = mgr.GetUPC(UPCName)?.GetUP(FirePointName)?.GetSubType<UnitPropertyVector2>().V2;

                if (v == null )
                {
                    firePoint = DeffFirePoint;
                }
                else
                {
                    Vector3 v3 = new Vector3(v.Value.x, 0, v.Value.y);
                    firePoint = v3;
                }

            }
        }
        private void getMuzzle()
        {
            if (!AssetsManager. Instance.TryLoadAsset(GameTool.GetStr(bulletPrefab.name, "_Muzzle"), out muzzlePrefab)) return;
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
                    var s = GameTool.GetStr("DL.Common.", v[i], "_Eff");
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
                //crListIeff[i].Execute(mgr);
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

            //子弹
            var go = GameObjectPool.Instance.CreateObject(bulletPrefab.name, bulletPrefab, mgr.transform.position + firePoint, mgr.transform.rotation, null, InitBullet);

            //播放效果列表
            var v = mgr.GetUP(UPCName, "FiredEffs");
            if (v != null) exeListIeff(v.stringVal);

            //效果 动画
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
    }

}
