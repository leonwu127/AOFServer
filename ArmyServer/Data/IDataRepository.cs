namespace ArmyServer.Data;

public interface IDataRepository<TKey, TModel>
{
    void Add(TKey key,TModel model);
    void Set(TKey key, TModel model);
    TModel? Get(TKey key);
    List<TModel> GetAll();
    bool Remove(TKey key);
    bool Exists(TKey key);
    
}
