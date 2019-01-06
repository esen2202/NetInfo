using NetAda.Commands.Generic;
using NetInfo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace NetAda.ViewModels
{
    public class ViewModelMainWindow : ViewModelBase
    {
        private static AdapterInfo adapterInfo;

        private ObservableCollection<AdapterObject> _listAdapter;

        public ObservableCollection<AdapterObject> ListAdapter
        {
            get { return _listAdapter; }
            set
            {
                _listAdapter = value;
                base.OnPropertyChanged(() => ListAdapter);
            }
        }

        private AdapterObject _currentAdapter;

        public AdapterObject CurrentAdapter
        {
            get { return _currentAdapter; }
            set
            {
                _currentAdapter = value;
                base.OnPropertyChanged(() => CurrentAdapter);
            }
        }

        public string GlobalIP
        {
            get { return GetPublicIpToString(); }
            set
            {
                base.OnPropertyChanged(() => GlobalIP);
            }
        }

        public ViewModelMainWindow()
        {
            GetAdapterList();
        }

        private void GetAdapterList()
        {
            ListAdapter = new ObservableCollection<AdapterObject>();

            if (adapterInfo != null) { adapterInfo.RefreshInfos(); }

            adapterInfo = AdapterInfo.CreateInstance();

            adapterInfo.listAdapter.ForEach(adapter =>
            {
                var adp = ListAdapter.Where(x => x.Name == adapter.Name).FirstOrDefault();
                if (adp != null)
                {
                    adp.IsDHCPEnabled = adapter.IsDHCPEnabled;
                }
                else
                {
                    ListAdapter.Add(adapter);
                }
            });

            ListAdapter = new ObservableCollection<AdapterObject>(ListAdapter.OrderByDescending(x => x.IsOperationalStatusUp));


            if(CurrentAdapter == null ) CurrentAdapter = ListAdapter.FirstOrDefault();
        }

        #region ICommand
        private ICommand _selectNetAdaptCommand;
        public ICommand SelectNetAdaptCommand
        {
            get
            {
                if (_selectNetAdaptCommand == null)
                {
                    _selectNetAdaptCommand = new RelayCommand(
                    p => true,
                    p => this.SelectedAdapter(p));
                }
                return _selectNetAdaptCommand;
            }
        }

        private void SelectedAdapter(object p)
        {
            if (p != null)
            {
                AdapterObject obj = p as AdapterObject;
                CurrentAdapter = obj;
            }
        }

        private ICommand _refreshExecuteCommand;
        public ICommand RefreshExecuteCommand
        {
            get
            {
                if (_refreshExecuteCommand == null)
                {
                    _refreshExecuteCommand = new RelayCommand(
                        p => true,
                        p =>
                        {
                            this.RefreshAdapters();
                            this.RefreshSpeed();
                            this.GlobalIP = "";

                        });
                }
                return _refreshExecuteCommand;
            }
        }

        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand
        {
            get
            {
                if (_closeWindowCommand == null)
                {
                    _closeWindowCommand = new RelayCommand(
                        p => true,
                        p => Application.Current.Shutdown());
                }
                return _closeWindowCommand;
            }
        }
        #endregion

        private void RefreshAdapters()
        {
            GetAdapterList();
        }

        private void RefreshSpeed()
        {
            if(adapterInfo !=null && CurrentAdapter != null)
            adapterInfo.RefreshAdapterSpeed(ref _currentAdapter);
        }

        public static string GetPublicIp()
        {
            string uri = "http://checkip.dyndns.org/";
            string ip = String.Empty;

            using (var client = new HttpClient())
            {
                var result = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;

                ip = result.Split(':')[1].Split('<')[0];
            }

            return ip;
        }

        public string GetPublicIpToString()
        {
            try
            {
                return GetPublicIp();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";

            }
            
        }
    }

}
