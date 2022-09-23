namespace RecyclableListView
{
    public interface IListItemPool
    {
        RecyclableListItemBase CreateItem();
        void StoreItem(RecyclableListItemBase item);
    }
}
