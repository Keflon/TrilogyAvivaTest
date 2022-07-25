using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Threading.Tasks;
using TrilogyAvivaTest.Services.Logging;
using TrilogyAvivaTest.Services.Misc;

namespace TrilogyAvivaTest.Services.Persistence
{
    public class KeyStore : IKeyStore
    {
        private readonly ILogger _logger;
        private string _cacheDir;

        private readonly AsyncAutoResetEvent _ar;

        public KeyStore(ILogger logger, string cacheDirectory)
        {
            _logger = logger;
            _cacheDir = cacheDirectory;

            if (!Directory.Exists(_cacheDir))
                Directory.CreateDirectory(_cacheDir);

            _ar = new AsyncAutoResetEvent(true);
        }

        bool _isBusy = false;

        public void ClearCache()
        {
            var things = Directory.GetFiles(_cacheDir);

            try
            {
                foreach (string item in things)
                {
                    File.Delete(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogLine($"KeyStore::ClearCache failed with {ex.Message}");
            }
        }

        public void Delete(string key)
        {
            try
            {
                var pathKey = GetKeyPath(key);
                File.Delete(pathKey);
            }
            catch (Exception ex)
            {
                _logger.LogLine($"KeyStore::Delete failed with {ex.Message}");
            }
        }

        public async Task<string> ReadStringAsync(string key)
        {
            try
            {
                await _ar.WaitAsync();

                if (_isBusy)
                {
                    throw new InvalidOperationException($"Broken read for key '{key}'");
                }
                _isBusy = true;

                var pathKey = GetKeyPath(key);

                if (File.Exists(pathKey))
                {
                    using (StreamReader thing = File.OpenText(pathKey))
                    {
                        var retval = await thing.ReadToEndAsync();
                        return retval;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogLine($"KeyStore::ReadStringAsync failed with {ex.Message}");
            }
            finally
            {
                _isBusy = false;
                _ar.Set();
            }
            return null;
        }

        public async Task<bool> WriteStringAsync(string key, string data)
        {
            try
            {
                await _ar.WaitAsync();
                if (_isBusy)
                {
                    throw new InvalidOperationException($"Logical error! Broken Write for key '{key}'");
                }
                _isBusy = true;

                Debug.WriteLine($"WriteStringAsync: {key} with data {data?.Substring(0, Math.Min(100, data?.Length ?? 0)) ?? "Null"}");

                using (StreamWriter thing = File.CreateText(GetKeyPath(key)))
                {
                    await thing.WriteAsync(data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogLine($"KeyStore::WriteStringAsync failed with {ex.Message}");
                return false;
            }
            finally
            {
                _isBusy = false;
                _ar.Set();
            }
        }

        private string GetKeyPath(string key)
        {
            var retval = Path.Combine(_cacheDir, key);
            return retval;
        }
    }
}
