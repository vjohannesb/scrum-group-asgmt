using SharedLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.StateContainers
{
    public class OrderState
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

        public event Action OnChange;

        public void NotifyStateChanged()
            => OnChange?.Invoke();

        public decimal CouponDiscount { get; set; }


    }
}
