using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DomainLib.Entities;
using Ninject.Activation.Caching;


namespace DomainLib
{
    public abstract class SettingsBase
    {
        // 1 name and properties cached in readonly fields
        private readonly string _name;
        private readonly PropertyInfo[] _properties;

        public SettingsBase()
        {
            var type = this.GetType();
            _name = type.Name;
            // 2
            _properties = type.GetProperties();
        }

        public virtual void Load(AppIdentityDbContext unitOfWork)
        {
            // ARGUMENT CHECKING SKIPPED FOR BREVITY
            // 3 get settings for this type name
            var settings = unitOfWork.Settings.Where(w => w.Type == _name).ToList();

            foreach (var propertyInfo in _properties)
            {
                // get the setting from the settings list
                var setting = settings.SingleOrDefault(s => s.Name == propertyInfo.Name);
                if (setting != null)
                {
                    // 4 assign the setting values to the properties in the type inheriting this class
                    propertyInfo.SetValue(this, Convert.ChangeType(setting.Value, propertyInfo.PropertyType));
                }
            }
        }

        public virtual void Save(AppIdentityDbContext unitOfWork)
        {
            // 5 load existing settings for this type
            var settings = unitOfWork.Settings.Where(w => w.Type == _name).ToList();

            foreach (var propertyInfo in _properties)
            {
                object propertyValue = propertyInfo.GetValue(this, null);
                string value = (propertyValue == null) ? null : propertyValue.ToString();

                var setting = settings.SingleOrDefault(s => s.Name == propertyInfo.Name);
                if (setting != null)
                {
                    // 6 update existing value
                    setting.Value = value;
                }
                else
                {
                    // 7 create new setting
                    var newSetting = new Setting()
                    {
                        Name = propertyInfo.Name,
                        Type = _name,
                        Value = value,
                    };
                    unitOfWork.Settings.Add(newSetting);
                }
            }
        }
    }
    public class GeneralSettings : SettingsBase
    {
        public string SiteName { get; set; }
        public string AdminEmail { get; set; }
    }

    public class SeoSettings : SettingsBase
    {
        public string HomeMetaTitle { get; set; }
        public string HomeMetaDescription { get; set; }
    }

    public interface ISettings
    {
        GeneralSettings General { get; }
        SeoSettings Seo { get; }
        void Save();
    }

    public class Settings : ISettings
    {
        // 1
        private readonly Lazy<GeneralSettings> _generalSettings;
        // 2
        public GeneralSettings General { get { return _generalSettings.Value; } }

        private readonly Lazy<SeoSettings> _seoSettings;
        public SeoSettings Seo { get { return _seoSettings.Value; } }

        private readonly AppIdentityDbContext _unitOfWork;
        private readonly ICache _cache;
        public Settings(AppIdentityDbContext unitOfWork, ICache cache)
        {
            // ARGUMENT CHECKING SKIPPED FOR BREVITY
            _unitOfWork = unitOfWork;
            _cache = cache;
            _generalSettings = new Lazy<GeneralSettings>(CreateSettingsWithCache<GeneralSettings>);
            _seoSettings = new Lazy<SeoSettings>(CreateSettingsWithCache<SeoSettings>);
        }

        private T CreateSettingsWithCache<T>() where T : SettingsBase, new()
        {
            // this is where you would implement loading from ICache
            throw new NotImplementedException();
        }
        public Settings(AppIdentityDbContext unitOfWork)
        {
            // ARGUMENT CHECKING SKIPPED FOR BREVITY
            _unitOfWork = unitOfWork;
            // 3
            _generalSettings = new Lazy<GeneralSettings>(CreateSettings<GeneralSettings>);
            _seoSettings = new Lazy<SeoSettings>(CreateSettings<SeoSettings>);
        }

        public void Save()
        {
            // only save changes to settings that have been loaded
            if (_generalSettings.IsValueCreated)
                _generalSettings.Value.Save(_unitOfWork);

            if (_seoSettings.IsValueCreated)
                _seoSettings.Value.Save(_unitOfWork);

            _unitOfWork.SaveChanges();
        }
        // 4
        private T CreateSettings<T>() where T : SettingsBase, new()
        {
            var settings = new T();
            settings.Load(_unitOfWork);
            return settings;
        }
    }
}
