using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UplanModels;

namespace GetInfo_Global_AccountService
{
    public class MyDbContext:DbContext
    {
        //增加 DbSet
        //用户账户表
        public DbSet<UserInfo> UserAccountTable { get; set; }
        //QoE设备表
        public DbSet<QoEDeviceInfo> QoEDeviceTable { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // 除了startup里面通过appsetting配置之外，还可以直接在此配置 如下：
            // server = 123.207.31.37; port = 3306; database = efTest; uid = root; password = Mei19931129; sslmode = none
            string conn = Module.MysqlConnstr;
            optionsBuilder.UseMySQL(conn);//配置连接字符串 必须加sslmode=none
        }

        public int Update<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties) where TEntity : class
        {
            var dbEntityEntry = this.Entry(entity);
            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                foreach (var property in dbEntityEntry.OriginalValues.Properties)
                {
                    string pName = property.Name;
                    var original = dbEntityEntry.OriginalValues.GetValue<object>(pName);
                    var current = dbEntityEntry.CurrentValues.GetValue<object>(pName);
                    if (original != null && !original.Equals(current))
                    {
                        dbEntityEntry.Property(pName).IsModified = true;
                    }
                }
            }
            return this.SaveChanges();
        }
    }
}
