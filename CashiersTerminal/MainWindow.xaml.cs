using CashiersTerminal.Database;
using CashiersTerminal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace CashiersTerminal
{
    public partial class MainWindow
    {
        private bool _sessionOpened;
        private bool _isStockOpened;
        private int _docNum;
        
        private List<StoreTable> _sessionGoods;
        private IQueryable<StoreTable> _goodsInStore;
        private double _selectedCost;

        public MainWindow()
        {
            // APP init:
            InitializeComponent();
            LoginBox.Focus();
            ContextFactory.SetConnectionParameters("localhost", "admin", "", "cashierbase"); // LOCAL access

            _docNum = GetDocNum();

            SelectGood.Visibility = Visibility.Hidden;
            Plus.Visibility = Visibility.Hidden;
            Minus.Visibility = Visibility.Hidden;
            Finish.Visibility = Visibility.Hidden;
            
            // Clocks timer:
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Clocks;
            dispatcherTimer.Interval = new TimeSpan(0,0,1);
            dispatcherTimer.Start();
        }

        private void Clocks(object sender, EventArgs e)
        {
            CurrentDateTime.Content = DateTime.Now.ToString("G");
        }

        // SHOW goods on the stock:
        private void SelectGood_Click(object sender, RoutedEventArgs e)
        {
            var storeList = new List<StoreTable>();
            _goodsInStore = ContextFactory.Instance.StoreTable.Where(x => x.Qty != 0);
            var dynamicId = 1;
            
            Products.ItemsSource = null;
            foreach (var good in _goodsInStore)
            {
                var newGood = new StoreTable(dynamicId, good.GoodName, good.Cost, good.Qty, good.Code);
                if (!_sessionOpened)
                    storeList.Add(newGood); // FIRST click
                if (_sessionOpened && _sessionGoods.All(x => x.Code != newGood.Code))
                    storeList.Add(newGood); // NEXT adding
                dynamicId++;
            }
            if (storeList.Count == 0)
            {
                Products.ItemsSource = _sessionGoods;
                Products.Items.Refresh();
                _isStockOpened = false;
                MessageBox.Show("Склад пуст или вами выбраны все доступные в нем товары.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
                
            Products.ItemsSource = storeList;

            // SESSION open trigger:
            if (!_sessionOpened)
            {
                _sessionOpened = true;
                _sessionGoods = new List<StoreTable>();
                InterfaceReInit();
            }
            _selectedCost = 0;
            _isStockOpened = true;
        }
        // ADD selected good to session order:
        private void Products_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // GET from stock (selected var):
            var selectedGood = Products.SelectedItem as StoreTable;
            if (selectedGood == null) return;

            // WORK with order:
            if (!_isStockOpened)
            {
                UpdateSelected(selectedGood);
                UpdateSum();
                return;
            }

            // WORK with stock (add from it to grid):
            selectedGood.Qty = 1;
            selectedGood.Id = _sessionGoods.Count + 1;
            UpdateSelected(selectedGood);

            Products.ItemsSource = null;
            _sessionGoods.Add(selectedGood);
            Products.ItemsSource = _sessionGoods;
            UpdateDoc();
            Products.Items.Refresh();
            UpdateSum();

            _isStockOpened = false;
        }
        // PLUS one qty in order:
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            var selectedGood = Products.SelectedItem as StoreTable;
            if (selectedGood == null) return;

            var selectedGoodOnStock = _goodsInStore.FirstOrDefault(x => x.Code == selectedGood.Code);
            if (selectedGoodOnStock == null) return;
            
            if (selectedGood.Qty >= selectedGoodOnStock.Qty)
            {
                MessageBox.Show("Товара на складе больше нет!",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            selectedGood.Qty += 1;
            selectedGood.Cost = _selectedCost * selectedGood.Qty;
            QtySmall.Content = $"{selectedGood.Qty:f2}  шт.";
            SumSmall.Content = $"{selectedGood.Cost:f2} руб.";
            Products.Items.Refresh();
            UpdateSum();
        }
        // MINUS one qty in order:
        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            var selectedGood = Products.SelectedItem as StoreTable;
            if (selectedGood == null) return;

            selectedGood.Qty -= 1;
            selectedGood.Cost = _selectedCost * selectedGood.Qty;
            QtySmall.Content = $"{selectedGood.Qty:f2}  шт.";
            SumSmall.Content = $"{selectedGood.Cost:f2} руб.";
            Products.Items.Refresh();

            if (selectedGood.Qty == 0)
            {
                //MessageBox.Show("Товар удален!");
                _sessionGoods.Remove(selectedGood);
                Products.Items.Refresh();
            }
            UpdateSum();
        }
        // FINISHING:
        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (_sessionGoods == null)
            {
                MessageBox.Show("Выберите товар со склада!",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (CashBox.Text == "")
            {
                MessageBox.Show("Введите вносимые наличные деньги!", 
                    "Предупреждение", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
                return;
            }
            var docQty = 0;
            var docSum = 0.0;

            foreach (var sessionGood in _sessionGoods)
            {
                foreach (var stockGood in _goodsInStore)
                {
                    if (sessionGood.Code == stockGood.Code)
                    {
                        if (sessionGood.Qty == stockGood.Qty)
                             ContextFactory.Instance.StoreTable.Remove(stockGood);
                        else stockGood.Qty -= sessionGood.Qty;
                    }
                }
                var cost = sessionGood.Cost / sessionGood.Qty;
                var addGood = new SalesTable(
                    sessionGood.GoodName,
                    cost,  
                    sessionGood.Qty,
                    sessionGood.Cost , 
                    sessionGood.Code,
                    _docNum);
                ContextFactory.Instance.SalesTable.Add(addGood);

                docQty += sessionGood.Qty;
                docSum += sessionGood.Cost;
            }

            var newDoc = new Docs(_docNum, docQty, docSum);
            ContextFactory.Instance.Docs.Add(newDoc);
            ContextFactory.Instance.SaveChanges();

            if (DocSumText.Content.ToString() != "ВЫДАТЬ СДАЧИ:") CashBack();

            var messageBoxFinish = MessageBox.Show(
                "Сессия закрыта.\nНомер документа: " + _docNum + "\nСумма документа: " + docSum, 
                "Успешно!", 
                MessageBoxButton.OK, 
                MessageBoxImage.Asterisk);

            if (messageBoxFinish == MessageBoxResult.OK)
            {
                _sessionGoods.Clear();
                _sessionOpened = false;
                _isStockOpened = true;

                InterfaceReInit();
                Products.Items.Refresh();

                _docNum = GetDocNum();
            }
        }

        // AUTH from button:
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        // AUTH from enter.key
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Login();
        }

        // PRIVATE utility methods:
        private void InterfaceReInit()
        {
            DocSumText.Content = "СУММА ДОКУМЕНТА:";
            DocType.Content = "Отсутствует";
            NumDoc.Content = 0;
            Position.Content = 0;
            PositionSmall.Content = "";
            CodeSmall.Content = "";
            PriceSmall.Content = "";
            QtySmall.Content = "";
            SumSmall.Content = "";
            CashBox.Text = "";
            FinishSum.Content = "00.00";
        }
        private void UpdateSelected (StoreTable product)
        {
            _selectedCost = product.Cost;
            PositionSmall.Content = product.GoodName;
            CodeSmall.Content = product.Code;
            PriceSmall.Content = $"{product.Cost:f2}  руб.";
            QtySmall.Content = $"{product.Qty:f2}  шт.";
            SumSmall.Content = $"{product.Cost:f2} руб.";
        }
        private void UpdateDoc()
        {
            DocType.Content = "Продажа (открыт)";
            NumDoc.Content = _docNum;
            Position.Content = _sessionGoods.Count;
        }
        private void UpdateSum()
        {
            var sum = 0.0;
            foreach (var good in _sessionGoods)
            {
                sum += good.Cost;
            }
            FinishSum.Content = $"{sum:f2}";
        }
        private int GetDocNum()
        {
            var docNum = 0;
            var docs = ContextFactory.Instance.Docs.Where(x => x.DocNum != 0);
            foreach (var doc in docs)
            {
                if (doc.DocNum > docNum) docNum = doc.DocNum;
            }
            docNum++;

            return docNum;
        }
        private void Login()
        {
            var login = LoginBox.Text;
            var user = ContextFactory.Instance.Users.FirstOrDefault(x => x.Login == login);
            if (user == null)
            {
                MessageBox.Show("Введите имя пользователя!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MD5 md5 = new MD5CryptoServiceProvider();
            var pass = PasswordBox.Password;
            var checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
            var result = BitConverter.ToString(checkSum).Replace("-", String.Empty);

            if (result == user.Password)
            {
                LoginLabel.Visibility = Visibility.Hidden;
                LoginBox.Visibility = Visibility.Hidden;
                PasswordLabel.Visibility = Visibility.Hidden;
                PasswordBox.Visibility = Visibility.Hidden;
                Go.Visibility = Visibility.Hidden;

                SelectGood.Visibility = Visibility.Visible;
                Plus.Visibility = Visibility.Visible;
                Minus.Visibility = Visibility.Visible;
                Finish.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Вы ввели неверный пароль!", 
                    "Ошибка", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        // CASH calculate
        private void CashBack()
        {
            DocSumText.Content = "ВЫДАТЬ СДАЧИ:";
            var cashBack = Convert.ToDouble(CashBox.Text) - Convert.ToDouble(FinishSum.Content);
            FinishSum.Content = $"{cashBack:f2}";
        }
        // CASH calculate from enter.key
        private void CashBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CashBack();
        }
    }
}
