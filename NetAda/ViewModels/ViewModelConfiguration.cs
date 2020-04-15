using Core;
using Model;
using NetAda.Commands.Generic;
using NetAda.Views;
using NetInfo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace NetAda.ViewModels
{
    public class ViewModelConfiguration : ViewModelBase
    {

        private ObservableCollection<AdapterConfiguration> _listAdapterConfiguration;

        public ObservableCollection<AdapterConfiguration> ListAdapterConfiguration
        {
            get { return _listAdapterConfiguration; }
            set
            {
                _listAdapterConfiguration = value;
                base.OnPropertyChanged(() => ListAdapterConfiguration);
            }
        }

        private string _assignResult;
        public string AssignResult
        {
            get { return _assignResult; }
            set
            {
                _assignResult = value;
                base.OnPropertyChanged(() => AssignResult);
            }
        }

        private AdapterConfiguration _currentAdapterConfiguration;

        public AdapterConfiguration CurrentAdapterConfiguration
        {
            get { return _currentAdapterConfiguration; }
            set
            {
                _currentAdapterConfiguration = value;
                base.OnPropertyChanged(() => CurrentAdapterConfiguration);
            }
        }

        public AdapterConfiguration assignAdapterConfiguration = null;

        private Model.DB configurationDB = new DB("AdapterConfigurationDB");

        private NetshSetIp netshSetIp;
        public ViewModelConfiguration()
        {
            GetAdapterConfigurationList();

        }
        private void GetAdapterConfigurationList()
        {
            ListAdapterConfiguration = new ObservableCollection<AdapterConfiguration>();

            var listRecords = configurationDB.ListRecords();

            listRecords.ForEach(record =>
            {
                ListAdapterConfiguration.Add(record);
            });

            ListAdapterConfiguration = new ObservableCollection<AdapterConfiguration>(ListAdapterConfiguration.OrderByDescending(x => x.GroupName));

            CurrentAdapterConfiguration = new AdapterConfiguration();

            //ListAdapterConfiguration = CreateListAdapterConfigurations();
        }

        private ObservableCollection<AdapterConfiguration> CreateListAdapterConfigurations()
        {

            return new ObservableCollection<AdapterConfiguration>()
            {
                new AdapterConfiguration()
                {
                   Id = 1, GroupName =  "Home",Name =  "Home Conf1", Description = "Home Description1" ,IpAddress = "192.168.0.10", SubnetMask = "255.255.255.0", Gateway = "192.168.0.1"
                },
                new AdapterConfiguration()
                {
                    Id = 2, GroupName =  "Home",Name =  "Home Conf2", Description = "Home Description2" ,IpAddress = "192.168.2.10", SubnetMask = "255.255.0.0", Gateway = "192.168.2.1",DNSServer2 = "192.0.0.1",DNSServer1 = "8.8.4.4"
                },
                new AdapterConfiguration()
                {
                    Id = 2, GroupName =  "Work",Name =  "Work Conf1", Description = "Work Description1" ,IpAddress = "10.10.2.10", SubnetMask = "255.255.0.0", Gateway = "10.10.2.1"
                }
            };
        }

        #region ICommand

        private ICommand _assignConfigurationCommand;
        public ICommand AssignConfigurationCommand
        {
            get
            {
                if (_assignConfigurationCommand == null)
                {
                    _assignConfigurationCommand = new RelayCommand(
                        p =>
                        {
                            AssignConfiguration(p);

                        });
                }
                return _assignConfigurationCommand;
            }
        }

        private void AssignConfiguration(object p)
        {
            if (p != null)
            {
                var obj = p as AdapterConfiguration;
                var dataContextMainWindow = ((ViewModelMainWindow)Application.Current.MainWindow.DataContext);

                netshSetIp = new NetshSetIp(dataContextMainWindow.CurrentAdapter.Name, obj);

                netshSetIp.OnProcessCompleted += AssignResultMethod;

                assignAdapterConfiguration = CurrentAdapterConfiguration.Clone();

            }
        }

        private ICommand _assignDHCPConfigurationCommand;
        public ICommand AssignDHCPConfigurationCommand
        {
            get
            {
                if (_assignDHCPConfigurationCommand == null)
                {
                    _assignDHCPConfigurationCommand = new RelayCommand(
                        p =>
                        {
                            AssignDHCPConfiguration(p);

                        });
                }
                return _assignDHCPConfigurationCommand;
            }
        }

        private void AssignDHCPConfiguration(object p)
        {
            if (p != null)
            {
                var obj = p as AdapterConfiguration;

                netshSetIp = new NetshSetIp(((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.Name);

                netshSetIp.OnProcessCompleted += AssignResultMethod;

                assignAdapterConfiguration = null;
            }
        }


        private void AssignResultMethod(object sender, EventArgs e)
        {
            AssignResult = netshSetIp.ResultData;
            Dispatcher.CurrentDispatcher.InvokeAsync(()=>
            {
                
                 
                if (assignAdapterConfiguration == null)
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.IsDHCPEnabled = true;
                else
                {
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.IsDHCPEnabled = false;
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.IpAddress = assignAdapterConfiguration.IpAddress;
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.SubnetMask = assignAdapterConfiguration.SubnetMask;
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.Gateway = assignAdapterConfiguration.Gateway;
                    ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.DNSServer1 = assignAdapterConfiguration.DNSServer1;
                }

                var result = ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).ListAdapter.Where(o => o.Name == ((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter.Name).FirstOrDefault();
                Core.Class.CopyObjectPropertiesValue(((ViewModelMainWindow)Application.Current.MainWindow.DataContext).CurrentAdapter, result);
            });
        }

        private ICommand _updateConfigurationCommand;
        public ICommand UpdateConfigurationCommand
        {
            get
            {
                if (_updateConfigurationCommand == null)
                {
                    _updateConfigurationCommand = new RelayCommand(
                        p =>
                        {
                            UpdateConfiguration(p);

                        });
                }
                return _updateConfigurationCommand;
            }
        }

        private void UpdateConfiguration(object p)
        {
            if (p != null)
            {
                var obj = p as AdapterConfiguration;

                configurationDB.UpdateRecord(obj);

                GetAdapterConfigurationList();
                CurrentAdapterConfiguration = obj;
            }
        }

        private ICommand _deleteConfigurationCommand;
        public ICommand DeleteConfigurationCommand
        {
            get
            {
                if (_deleteConfigurationCommand == null)
                {
                    _deleteConfigurationCommand = new RelayCommand(
                        p =>
                        {
                            DeleteConfiguration(p);

                        });
                }
                return _deleteConfigurationCommand;
            }
        }

        private void DeleteConfiguration(object p)
        {
            if (p != null)
            {
                var obj = p as AdapterConfiguration;

                configurationDB.DeleteRecord(obj.Id);
                GetAdapterConfigurationList();
            }
        }

        private ICommand _addConfigurationCommand;
        public ICommand AddConfigurationCommand
        {
            get
            {
                if (_addConfigurationCommand == null)
                {
                    _addConfigurationCommand = new RelayCommand(
                        p =>
                        {
                            AddConfiguration(p);

                        });
                }
                return _addConfigurationCommand;
            }
        }

        private void AddConfiguration(object p)
        {
            if (p != null)
            {
                var obj = p as AdapterConfiguration;

                configurationDB.AddRecord(ref obj);
                GetAdapterConfigurationList();
            }
        }
        #endregion
    }
}
