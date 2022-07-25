using System.Threading.Tasks;

namespace TrilogyAvivaTest.Services.Persistence
{
    public interface IKeyStore
    {
        /// <summary>
        /// Stores a string against a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> WriteStringAsync(string key, string data);
        /// <summary>
        /// Returns a string stored against a key or null if the key was not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> ReadStringAsync(string key);
        /// <summary>
        /// Clears any stored keys
        /// </summary>
        void ClearCache();
        /// <summary>
        /// Removes a string stored against a given key
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);

        // TODO: String streams etc.
    }
}
