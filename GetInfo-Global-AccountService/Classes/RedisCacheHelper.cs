﻿using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UplanModels;

namespace GetInfo_Global_AccountService
{
    public static class RedisConfig
    {
        public static string Configname { get; set; }
        public static string Connection { get; set; }
        public static int DefaultDatabase { get; set; }
        public static string InstanceName { get; set; }
    }
    public class RedisCacheHelper : ICacheHelper
    {
        public static RedisCacheOptions options;
        public static IDatabase _cache;

        public static ConnectionMultiplexer _connection;

        public static string _instanceName;

        public static ISubscriber _sub;
        public RedisCacheHelper(/*RedisCacheOptions options, int database = 0*/)//这里可以做成依赖注入，但没打算做成通用类库，所以直接把连接信息直接写在帮助类里
        {
            options = new RedisCacheOptions();
            options.Configuration = RedisConfig.Connection;//RedisConfig.Connection;
            options.InstanceName = RedisConfig.InstanceName;
            
            int database = RedisConfig.DefaultDatabase;
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(database);
            _instanceName = options.InstanceName;
            _sub = _connection.GetSubscriber();
        }

        /// <summary>
        /// 取得redis的Key名称
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetKeyForRedis(string key)
        {
            return _instanceName + key;
        }
        /// <summary>
        /// 判断当前Key是否存在数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            return _cache.KeyExists(GetKeyForRedis(key));
        }

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            var value = _cache.StringGet(GetKeyForRedis(key));
            if (!value.HasValue)
                return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (Exists(GetKeyForRedis(key)))
                RemoveCache(GetKeyForRedis(key));

            _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value));
        }
        /// <summary>
        /// 设置绝对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiressAbsoulte"></param>
        public void SetCache(string key, object value, DateTime expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (Exists(GetKeyForRedis(key)))
                RemoveCache(GetKeyForRedis(key));
            TimeSpan t = expiressAbsoulte - TimeUtil.Now();
            _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value), t);
        }
        /// <summary>
        /// 设置相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationMinute"></param>
        public void SetCache(string key, object value, int expirationMinute)
        {
            if (Exists(GetKeyForRedis(key)))
                RemoveCache(GetKeyForRedis(key));
            TimeSpan ts = TimeSpan.FromMinutes(expirationMinute);
            _cache.StringSet(GetKeyForRedis(key), JsonConvert.SerializeObject(value), ts);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="endPoint"></param>
        /// <param name="database"></param>
        /// <param name="timeountseconds"></param>
        public void KeyMigrate(string key, EndPoint endPoint, int database, int timeountseconds)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            _cache.KeyMigrate(GetKeyForRedis(key), endPoint, database, timeountseconds);
        }
        /// <summary>
        /// 移除redis
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _cache.KeyDelete(GetKeyForRedis(key));
        }

        /// <summary>
        /// 销毁连接
        /// </summary>
        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public void GetMssages(string channelName)
        {
            using (_connection = ConnectionMultiplexer.Connect(options.Configuration))
                _sub.Subscribe(channelName, (channel, message) =>
                {
                    string result = message;
                });
        }

        public void Publish(string channelName,string msg)
        {
            using (_connection = ConnectionMultiplexer.Connect(options.Configuration))
                _sub.Publish(channelName, msg);
        }

    }
}
