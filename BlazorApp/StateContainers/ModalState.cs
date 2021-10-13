using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.StateContainers
{
    public class ModalState
    {
        private ProductViewModel _modalProduct = new();
        public ProductViewModel ModalProduct
        {
            get => _modalProduct;
            set
            {
                _modalProduct = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        public void NotifyStateChanged()
            => OnChange?.Invoke();
    }
}
