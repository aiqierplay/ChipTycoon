#if UNITY_EDITOR

namespace Aya.TweenPro
{
    public class TweenRowData : GUITableRowData<TweenRowData, Tweener>
    {
        public Tweener Tweener => Data;
        public TweenAnimation Animation => Tweener.Animation;
        public UTweenPlayer TweenPlayer => Animation.TweenPlayer;

        public bool Active;

        [GUITableColumn(Index = 0, Name = "ID", Width = 35)]
        public TweenCellID CellID;

        [GUITableColumn(Name = "Type", Width = 150, MinWidth = 100, MaxWidth = 400)]
        public TweenCellType CellType;

        [GUITableColumn(Name = "Player", Width = 200, MinWidth = 200, MaxWidth = 400)]
        public TweenCellPlayer Player;

        [GUITableColumn(Name = "Play Mode", Width = 90)]
        public TweenCellPlayMode PlayMode;

        [GUITableColumn(Name = "Update", Width = 70)]
        public TweenCellUpdateMode UpdateMode;

        [GUITableColumn(Name = "Auto", Width = 60)]
        public TweenCellAutoPlayMode AutoPlayMode;

        [GUITableColumn(Name = "Sample", Width = 60)]
        public TweenCellPreSampleMode PreSampleMode;

        [GUITableColumn(Name = "Time", Width = 70)]
        public TweenCellTimeMode TimeMode;

        [GUITableColumn(Name = "Duration")]
        public TweenCellDuration Duration;

        [GUITableColumn(Name = "Delay")]
        public TweenCellDelay Delay;

        [GUITableColumn(Name = "Ease", Width = 100)]
        public TweenCellEase Ease;

        [GUITableColumn(Name = "Progress", Width = 100, MinWidth = 100, MaxWidth = 100, CanSort = false)]
        public TweenCellProgress Progress;

        public TweenRowData(int id, Tweener data, bool active) : base(id, data)
        {
            Active = active;
        }

        public override void DrawCell(int columnIndex)
        {
            using (GUIEnableArea.Create(Active))
            {
                base.DrawCell(columnIndex);
            }
        }
    }
}
#endif