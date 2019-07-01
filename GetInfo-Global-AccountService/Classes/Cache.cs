using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetInfo_Global_AccountService
{
    public class CacheManager
    {
        private static readonly object lockobj = new object();
        private static volatile Cache _cache = null;
        /// <summary>
        /// Cache
        /// </summary>
        public static Cache CacheObj
        {
            get
            {
                if (_cache == null)
                {
                    lock (lockobj)
                    {
                        if (_cache == null)
                            _cache = new Cache();
                    }
                }
                return _cache;
            }
        }
    }
    public sealed class Cache
    {
        internal Cache() { }
        public static ICacheHelper cache;
        public void CheckCacheHelper()
        {
            if (cache != null && typeof(RedisCacheHelper).Equals(cache.GetType()))
            {
                //cache正常，无需重新设置
            }
            else
            {
                cache = new RedisCacheHelper();
            }
        }
        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            CheckCacheHelper();
            return cache.Exists(key);           
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">转换的类</typeparam>
        /// <param name="key">键名</param>
        /// <returns>转换为T类型的值</returns>
        public T GetCache<T>(string key) where T : class
        {
            CheckCacheHelper();
            return cache.GetCache<T>(key);          
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void Save(string key, object value)
        {
            CheckCacheHelper();
            cache.SetCache(key, value);          
        }

        /// <summary>
        /// 保存缓存并设置绝对过期时间
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="expMinute">过期分钟数</param>
        public void Save(string key, object value, int expMinute)
        {
            CheckCacheHelper();
            cache.SetCache(key, value, expMinute);          
        }      

        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <param name="key">要删除的key</param>
        public void Delete(string key)
        {
            CheckCacheHelper();
            cache.RemoveCache(key);           
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose() 
        {
            CheckCacheHelper();
            cache.Dispose();          
        }

        public void GetMessage() 
        {
            CheckCacheHelper();
            cache.GetMssages("msg");          
        }

        public void Publish(string msg) 
        {
            CheckCacheHelper();
            cache.Publish("msg", msg);            
        }
    }
}
