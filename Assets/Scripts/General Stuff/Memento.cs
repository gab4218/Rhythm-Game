using System.Collections.Generic;

public class Memento
{
    private List<MementoParameters> _memory = new();

    private int _limit;

    public Memento(int lim = 1000)
    {
        _limit = lim;
    }

    
    public object[] Remember()
    {
        if (_memory.Count <= 0) return default;

        object[] res = _memory[_memory.Count - 1].parameters;
        _memory.RemoveAt(_memory.Count - 1);

        return res;

    }

    public void Record(params object[] par)
    {
        _memory.Add(new(par));

        if (_memory.Count > _limit) _memory.RemoveAt(0);    
    }

    public object[] Peek() => _memory[_memory.Count - 1]?.parameters;

    public void Clear() => _memory.Clear();

    public bool CanRemember() => _memory.Count > 0;

    public int Count() => _memory.Count;

}



public class MementoParameters
{
    public object[] parameters;

    public MementoParameters(params object[] p)
    {
        parameters = p;
    }
}
