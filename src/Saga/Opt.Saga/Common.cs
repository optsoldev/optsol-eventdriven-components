using Newtonsoft.Json;

namespace Opt.Saga.Core
{
    public enum SagaState
    {
        Pendind,
        Running,
        Completed
    }


    public interface IStorageService
    {
        void PersistAsync(string key, object value);
    }
    public class InMemoryStorageService
    {
        static IDictionary<string, object> _storage = new Dictionary<string, object>();

        public InMemoryStorageService()
        {
        }

        public static void Add(string key, object value)
        {
            _storage.Add(key, value);
        }
        public static bool Contains(string id) => _storage.ContainsKey(id);
        public static object Get(string id) => _storage[id];
        public static object Remove(string id) => _storage[id];

    }
}