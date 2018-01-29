using System;

namespace DemoBindings
{
    public class ItemViewModel : Observable
    {
        private Action<ItemViewModel> _notifyOnSelectedChanged;

        public ItemViewModel(Item item, Action<ItemViewModel> notifyOnSelectedChanged)
        {
            Title = item.Title;
            _notifyOnSelectedChanged = notifyOnSelectedChanged;
        }

        public string Title { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value)
                    return;

                SetProperty(ref isSelected, value);

                if(value == true)
                    _notifyOnSelectedChanged?.Invoke(this);
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
