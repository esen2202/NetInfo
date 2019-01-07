using Model;
using NetAda.Commands.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

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

        private Model.DB configurationDB = new DB("AdapterConfigurationDB");

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

            ListAdapterConfiguration = new ObservableCollection<AdapterConfiguration>(ListAdapterConfiguration.OrderByDescending(x => x.Group));

            CurrentAdapterConfiguration = new AdapterConfiguration();
            
            //ListAdapterConfiguration = CreateListAdapterConfigurations();
        }

        private ObservableCollection<AdapterConfiguration> CreateListAdapterConfigurations()
        {

            return new ObservableCollection<AdapterConfiguration>()
            {
                new AdapterConfiguration()
                {
                   Id = 1, Group =  "Home",Name =  "Home Conf1", Description = "Home Description1" ,IpAddress = "192.168.0.10", SubnetMask = "255.255.255.0", Gateway = "192.168.0.1"
                },
                new AdapterConfiguration()
                {
                    Id = 2, Group =  "Home",Name =  "Home Conf2", Description = "Home Description2" ,IpAddress = "192.168.2.10", SubnetMask = "255.255.0.0", Gateway = "192.168.2.1",DNSServer2 = "192.0.0.1",DNSServer1 = "8.8.4.4"
                },
                new AdapterConfiguration()
                {
                    Id = 2, Group =  "Work",Name =  "Work Conf1", Description = "Work Description1" ,IpAddress = "10.10.2.10", SubnetMask = "255.255.0.0", Gateway = "10.10.2.1"
                }
            };
        }

        #region ICommand
        private ICommand _addConfigurationCommand;
        public ICommand AddConfigurationCommand
        {
            get
            {
                if (_addConfigurationCommand == null)
                {
                    _addConfigurationCommand = new RelayCommand(
                        p => true,
                        p => AddConfiguration(p));
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

            }
        }


        #endregion
    }
}
