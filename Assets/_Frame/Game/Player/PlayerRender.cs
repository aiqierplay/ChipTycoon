using Aya.Extension;
using Aya.Particle;
using UnityEngine;

public class PlayerRender : PlayerBase
{
    public Transform RenderTrans;

    [SubPoolInstance] public GameObject RenderInstance { get; set; }

    public override void InitComponent()
    {
        RenderTrans.SetLocalPositionX(0f);
        // RefreshRender(State.Point);
    }

    #region Render with Point
  
    public void RefreshRender(int point)
    {
        var dataList = PlayerSetting.Ins.PlayerDatas;
        var rank = 0;
        var data = dataList[0];
        for (var i = 0; i < dataList.Count; i++)
        {
            if (point >= dataList[i].Point)
            {
                data = dataList[i];
                rank = i;
            }
        }

        if (State.Rank != rank)
        {
            State.Rank = rank;
            Self.Data = data;

            var playerRendererPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
            RefreshRender(playerRendererPrefab);

            this.ExecuteNextFrame(() =>
            {
                if (data.ChangeFx != null && State.PointChanged)
                {
                    SpawnFx(data.ChangeFx, RenderTrans);
                }
            });
        }
    } 
    #endregion

    public void RefreshRender(GameObject prefab)
    {
        DeSpawnRenderer();
        RenderInstance = GamePool.Spawn(prefab, RenderTrans);

        ComponentDic.ForEach(c =>
        {
            var component = c.Value;
            component.CacheRendererComponent();
            component.CacheAnimator();
        });
    }

    public void DeSpawnRenderer()
    {
        if (RenderInstance != null)
        {
            GamePool.DeSpawn(RenderInstance);
            RenderInstance = null;
        }
    }

    #region Fx

    public override ParticleSpawner SpawnFx(GameObject fxPrefab)
    {
        return SpawnFx(fxPrefab, RenderTrans);
    }

    #endregion
}
