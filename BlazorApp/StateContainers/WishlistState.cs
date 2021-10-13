using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.StateContainers
{
    public class WishlistState
    {
        private List<ProductViewModel> _wishlistProducts = new();
        public List<ProductViewModel> WishlistProducts
        {
            get => _wishlistProducts;
            set
            {
                _wishlistProducts = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        public void NotifyStateChanged()
            => OnChange?.Invoke();
    }
}
