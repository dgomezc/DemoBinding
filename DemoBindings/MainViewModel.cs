using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DemoBindings
{
    public class MainViewModel : Observable
    {
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public MainViewModel()
        {
            var items = ItemsService.GetItems();
            var itemsVM = items.Select(i => new ItemViewModel(i, (item) => ChangeSelectedItem(item)));
            Items = new ObservableCollection<ItemViewModel>(itemsVM);

            Items.First().IsSelected = true;
        }

        private ItemViewModel _selectedItem;


        public ItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
                    return;

                Items.FirstOrDefault(i => i == value).IsSelected = true;
            }
        }

        private void ChangeSelectedItem(ItemViewModel newItem)
        {
            if (CanSelectedChanged())
            {
                Items.Where(i => i != newItem).ToList().ForEach(i => i.IsSelected = false);
                SetProperty(ref _selectedItem, newItem, nameof(SelectedItem));
            }
            else
            {
                //revert
                newItem.IsSelected = false;

                //this is the truqui, setear antes, tirar pa tras y hacer el notify en un dispatcher
                var originalItem = _selectedItem;
                _selectedItem = newItem;

                Application.Current.Dispatcher.BeginInvoke(
                new Action(() => SetProperty(ref _selectedItem, originalItem, nameof(SelectedItem))),
                DispatcherPriority.ContextIdle,
                null);
            }
        }

        private bool CanSelectedChanged()
        {
            if (!HasChanges)
                return true;

            return MessageBox.Show(
                "Are you sure to change the item?", 
                "Change Item", 
                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }


        private bool _hasChanges;
        public bool HasChanges
        {
            get => _hasChanges;
            set => SetProperty(ref _hasChanges, value);
        }


        private ICommand _command;
        public ICommand MyCommand => _command ?? (_command = new RelayCommand(() => ExecuteCommand()));

        private void ExecuteCommand()
        {
            ListViewSelectedItem.IsSelected = true;
        }

        public ItemViewModel ListViewSelectedItem { get; set; }
    }
}
