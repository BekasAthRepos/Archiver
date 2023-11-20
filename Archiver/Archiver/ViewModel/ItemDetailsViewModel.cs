using Archiver.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Archiver.ViewModel
{
    public class ItemDetailsViewModel: INotifyPropertyChanged
    {
        private Item _item;
        private bool _isSync;
        public event PropertyChangedEventHandler PropertyChanged;
        public Item Item 
        { 
            get { return _item; }
            set 
            { 
                _item = value;
                OnPropertyChanged("Item");
            }
        }

        public ItemDetailsViewModel(Item item, bool isSync) 
        {
            _item = item;
            _isSync = isSync;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
