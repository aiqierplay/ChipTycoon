#if UNITY_EDITOR

namespace Aya.TweenPro
{
    public abstract class GUITableCell<TRowData, TData>
        where TRowData : GUITableRowData<TRowData, TData>
    {
        public TRowData RowData;
        public TData Data;

        public void Init(TRowData rowData)
        {
            RowData = rowData;
            Data = rowData.Data;
        }

        public abstract void DrawValue();

        public virtual int CompareValue(TData data1, TData data2)
        {
            return 0;
        }
    }

    public abstract class GUITableCell<TRowData, TData, TValue> : GUITableCell<TRowData, TData>
        where TRowData : GUITableRowData<TRowData, TData>
    {
        public virtual TValue GetValue()
        {
            return default;
        }

        public virtual void SetValue(TValue value)
        {

        }
    }
}
#endif