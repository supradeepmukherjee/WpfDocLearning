using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDocLearning.Helpers;
using WpfDocLearning.Models;

namespace WpfDocLearning.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); UpdateStatus(); }
        }

        private string _newItemName = "";
        public string NewItemName
        {
            get => _newItemName;
            set { _newItemName = value; OnPropertyChanged(nameof(NewItemName)); AddItemCommand.RaiseCanExecuteChanged(); }
        }

        public RelayCommand AddItemCommand { get; }
        public RelayCommand RemoveSelectedCommand { get; }

        private string _statusMessage = "Ready";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); } }

        public RelayCommand ToggleActiveCommand { get; }

        public MainViewModel()
        {
            Items.Add(new Item { Name = "First item", IsActive = true });
            Items.Add(new Item { Name = "Second item", IsActive = false });

            AddItemCommand = new RelayCommand(_ => AddItem(), _ => !string.IsNullOrWhiteSpace(NewItemName));
            RemoveSelectedCommand = new RelayCommand(_ => RemoveSelected(), _ => SelectedItem != null);

            var parent1 = new Item { Name = "Parent 1", IsActive = true, Children = new ObservableCollection<Item>() };
            parent1.Children.Add(new Item { Name = "Child 1.1", IsActive = false });
            parent1.Children.Add(new Item { Name = "Child 1.2", IsActive = true });
            var parent2 = new Item { Name = "Parent 2", IsActive = false, Children = new ObservableCollection<Item>() };
            parent2.Children.Add(new Item { Name = "Child 2.1", IsActive = false });

            Items.Add(parent1);
            Items.Add(parent2);
            Items.Add(new Item { Name = "Standalone", IsActive = false });

            AddItemCommand = new RelayCommand(_ => AddItem(), _ => !string.IsNullOrWhiteSpace(NewItemName));
            RemoveSelectedCommand = new RelayCommand(_ => RemoveSelected(), _ => SelectedItem != null);

            ToggleActiveCommand = new RelayCommand(_ =>
            {
                if (SelectedItem != null) SelectedItem.IsActive = !SelectedItem.IsActive;
            }, _ => SelectedItem != null);
        }

        private void AddItem()
        {
            var item = new Item { Name = NewItemName, IsActive = false };
            Items.Add(item);
            SelectedItem = item;
            NewItemName = "";
            UpdateStatus();
            RemoveSelectedCommand.RaiseCanExecuteChanged();
        }

        private void RemoveSelected()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = Items.FirstOrDefault();
                UpdateStatus();
                RemoveSelectedCommand.RaiseCanExecuteChanged();
            }
        }

        private void UpdateStatus()
        {
            StatusMessage = SelectedItem == null ? $"Items count: {Items.Count}" : $"Selected: {SelectedItem.Name} (Active: {SelectedItem.IsActive})";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
