using SampleCSharpUI.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SampleCSharpUI.Models
{
    internal class SettingsModel : INotifyPropertyChanged
    {
        private string _ClientId = Config.ClientId;
        public string ClientId
        {
            get { return _ClientId; }
            set
            {
                if (_ClientId != value)
                {
                    _ClientId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _TenantName = Config.TenantName;
        public string TenantName
        {
            get { return _TenantName; }
            set
            {
                if (_TenantName != value)
                {
                    _TenantName = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsModel()
        {
            Config.LoadProperties();
        }

        /// <summary>
        /// 設定画面の値を保存する
        /// </summary>
        internal async Task SaveAsync()
        {
            if (Config.TenantName != this.TenantName || Config.ClientId != this.ClientId)
            {
                // 設定が変更された場合はログアウトする
                await App.MainVM.DisconnectAsync();
            }

            // 設定を保存する
            Config.TenantName = this.TenantName;
            Config.ClientId = this.ClientId;
            Config.SaveProperties();
        }

        // プロパティが変更されたときに通知するイベント
        public event PropertyChangedEventHandler PropertyChanged;

        // プロパティ変更通知を発行するメソッド
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
