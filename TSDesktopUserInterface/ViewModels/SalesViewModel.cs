using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSDesktopUserInterfaceLibrary.API;
using TSDesktopUserInterfaceLibrary.Models;

namespace TSDesktopUserInterface.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
            
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set 
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private int _itemQuantity;

        //Caliburn.Mirco will validate whether the Text box has a valid int or not
        //in the View page. Thus, we can put int instead of string for a TextBox
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);

            }
        }

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set 
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        public string SubTotal
        {
            get 
            { 

                //TODO - Replace with calculation
                return "$0.00"; 
            }
     
        }


        public string Tax
        {
            get
            {

                //TODO - Replace with calculation
                return "$0.00";
            }

        }

        public string Total
        {
            get
            {

                //TODO - Replace with calculation
                return "$0.00";
            }

        }



        public bool CanAddToCart
        {
            get
            {

                bool output = false;

                //make sure something is selected
                //make sure there is an item quantity

                return output;
            }
        }

        public void AddToCart()
        {

        }

        public bool CanRemoveFromCart
        {
            get
            {

                bool output = false;

                //make sure something is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {

        }

        public bool CanCheckOut
        {
            get
            {

                bool output = false;

                //make sure there is something in the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }

    }
}
