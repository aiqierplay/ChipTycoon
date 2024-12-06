using Aya.Physical;

public abstract class BuildingBase : EntityBase
{
    public TriggerArea TriggerArea;

    // [NonSerialized] public List<Worker> WorkerList = new List<Worker>();

    public virtual void Init()
    {
        if (TriggerArea != null)
        {
            TriggerArea.Enter.onTriggerEnter.Add<Worker>(OnEnter, LayerManager.Ins.Player);
            TriggerArea.Exit.onTriggerExit.Add<Worker>(OnExit, LayerManager.Ins.Player);
        }
    }

    public virtual void Refresh()
    {

    }

    public virtual void OnEnter(Worker worker)
    {
        // WorkerList.Add(worker);
        if (worker.Type != WorkerType.Player) return;
        worker.OnEnter(this);
        OnEnterImpl(worker);
        TriggerArea.OnEnter();
    }

    public virtual void OnExit(Worker worker)
    {
        // WorkerList.Remove(worker);
        if (worker.Type != WorkerType.Player) return;
        worker.OnExit(this);
        OnExitImpl(worker);
        TriggerArea.OnExit();
    }

    public abstract void OnEnterImpl(Worker worker);
    public abstract void OnExitImpl(Worker worker);
}
