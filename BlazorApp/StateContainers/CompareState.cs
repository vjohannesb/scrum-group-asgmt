using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.StateContainers
{
    public class CompareState
    {
        private List<ProductViewModel> _compareProducts;

        public List<ProductViewModel> CompareProducts
        {
            get => _compareProducts;
            set
            {
                _compareProducts = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        public void NotifyStateChanged()
        => OnChange?.Invoke();
    }
}
