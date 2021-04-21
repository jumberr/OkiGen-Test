using System.Collections.Generic;

namespace Classes
{
    public class Builder<T>
        where T : GeneratedObjects
    {
        private List<T> listObj;
        private List<List<T>> listGroups;
        
        public List<T> ListObj => listObj;
        public List<List<T>> ListGroups => listGroups;

        protected Builder()
        {
            listObj = new List<T>();
            listGroups = new List<List<T>>();
        }

        protected void AddNewObject(T obj)
        {
            listObj.Add(obj);
        }
        protected void AddNewList(List<T> list)
        {
            listGroups.Add(list);
        }

        protected void DelObj(int index)
        {
            if (index < listObj.Count && index >= 0)
            {
                listObj.RemoveAt(index);
            }
        }

        protected void CreateNewEmptyList()
        {
            listObj = new List<T>();
        }

        public T GetObj(List<T> list, int index) => list[index];
        public T GetObj(List<List<T>> list, int i, int j) => list[i][j];
    }
}