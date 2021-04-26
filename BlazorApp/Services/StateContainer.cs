using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public class StateContainer
    {
        public OrderViewModel CompletedOrder { get; set; }

        private List<ProductViewModel> _shoppingCartProducts;
        public List<ProductViewModel> ShoppingCartProducts
        {
            get => _shoppingCartProducts;
            set
            {
                _shoppingCartProducts = value;
                NotifyStateChanged();
            }
        }


        private ProductViewModel _modalProduct;
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
