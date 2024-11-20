
namespace Aya.DragDrop
{
    public abstract class DragAreaBase : DragDropBase
    {
        #region MonoBehaviour

        protected virtual void OnEnable()
        {
            Register();
        }

        protected virtual void OnDisable()
        {
            DeRegister();
        }

        #endregion

        #region Register / DeRegister

        public override void Register()
        {
            Group.AreaList.Add(this);
        }

        public override void DeRegister()
        {
            Group.AreaList.Remove(this);
        }

        #endregion

        public abstract bool CheckInArea(DragItemBase item);
    }
}