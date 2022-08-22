using System;
using System.Collections.Generic;

namespace Delivery
{
    public enum CouriersNames
    {
        Dmitriy,
        Irma,
        Ian
    }
    abstract class Delivery
    {
        protected string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
        protected int ArrivalHours;

        virtual protected void ShowArrivalHours()
        {
            Console.WriteLine($"Посылка прибудет через {ArrivalHours} часов по адресу: {address}");
        }
    }
    class Courier
    {
        private Random _rand = new Random();
        private CouriersNames _name;
        public CouriersNames Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }
        public Courier()
        {
            Name = (CouriersNames)_rand.Next(0, 2);
        }
    }

    class HomeDelivery : Delivery
    {
        private Courier _courier;
        public HomeDelivery()
        {
            Console.WriteLine("Введите ваш адрес");
            Address = Console.ReadLine();
            ArrivalHours = 5;
            _courier = new Courier();
            ShowArrivalHours();
        }
        public HomeDelivery(string address)
        {
            Address = address;
            ArrivalHours = 5;
            _courier = new Courier();
            ShowArrivalHours();
        }

        protected override void ShowArrivalHours()
        {
            base.ShowArrivalHours();
            Console.WriteLine($"Ваш курьер - {_courier.Name}");
        }
    }

    class PickPointDelivery : Delivery
    {
        public PickPointDelivery()
        {
            Address = "PickPoint";
            ArrivalHours = 3;
            ShowArrivalHours();
        }

    }

    class ShopDelivery : Delivery
    {
        public ShopDelivery()
        {
            Address = "Shop";
            ShowArrivalHours();
        }
        protected override void ShowArrivalHours()
        {
            Console.WriteLine($"Ваш заказ уже собран и ждёт вас в магазине по адресу: {Address}");
        }
    }
    abstract class Product
    {
        public string Type;
        public string Description;

        public void ShowInfo()
        {
            Console.WriteLine($"Тип продукта - {Type}\nОписание продукта - {Description}");
        }
    }
    class Food : Product
    {
        public Food()
        {
            Description = "Это можно съесть, приятного аппетита";
            Type = "Еда";
        }
    }
    class Furniture : Product
    {
        public Furniture()
        {
            Description = "Этим можно обустроить жилище";
            Type = "Мебель";
        }
    }
    class Clothes : Product
    {
        public Clothes()
        {
            Description = "Это можно надеть";
            Type = "Одежда";
        }
    }
    class Electronics : Product
    {
        public Electronics()
        {
            Description = "Это питается от электричества";
            Type = "Электроника";
        }

    }
    class User
    {
        private string _name;
        private List<Order<HomeDelivery>> _homeOrders;
        private List<Order<PickPointDelivery>> _pointOrders;
        private List<Order<ShopDelivery>> _shopOrders;
        private int _ordersCount;

        public User(string name)
        {
            _ordersCount = 0;
            _name = name;
            _homeOrders = new List<Order<HomeDelivery>>();
            _pointOrders = new List<Order<PickPointDelivery>>();
            _shopOrders = new List<Order<ShopDelivery>>();
        }
        public void MakeOrder()
        {
            Console.Clear();
            Console.WriteLine("Выберите тип доставки:\n1 - На дом\n2 - В пункт выдачи\n3 - В магазин");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Console.WriteLine("Введите ваш адрес:");
                    string address = Console.ReadLine();
                    AddOrder(_homeOrders);
                    _homeOrders[_homeOrders.Count - 1].Delivery = new HomeDelivery(address);
                    break;
                case "2":
                    AddOrder(_pointOrders);
                    _pointOrders[_pointOrders.Count - 1].Delivery = new PickPointDelivery();
                    break;
                case "3":
                    AddOrder(_shopOrders);
                    _shopOrders[_shopOrders.Count - 1].Delivery = new ShopDelivery();
                    break;
            }
            Console.Clear();
        }
        private void AddOrder<TDelivery>(List<Order<TDelivery>> listOrders) where TDelivery : Delivery
        {
            Console.Clear();
            Order<TDelivery> order = new Order<TDelivery>(_ordersCount + 1);
            listOrders.Add(order);
            _ordersCount++;
        }
        private void ShowOrder<TDelivery>(List<Order<TDelivery>> listOrders) where TDelivery : Delivery
        {
            foreach (var order in listOrders)
            {
                Console.WriteLine($"Заказ: \n{order.Id} - Адрес:{order.Delivery.Address}");

                foreach (var product in order.Products)
                {
                    Console.WriteLine($"{product.Type} - {product.Description}");
                }
            }
            Console.WriteLine();
        }
        public void ShowAllOrders()
        {
            Console.Clear();
            Console.WriteLine("Заказы на дом:");
            ShowOrder(_homeOrders);
            Console.WriteLine("Заказы в пункты выдачи:");
            ShowOrder(_pointOrders);
            Console.WriteLine("Заказы в магазин:");
            ShowOrder(_shopOrders);
        }
    }
    class Order<TDelivery> where TDelivery :  Delivery
    {
        public TDelivery Delivery;
        public int Id;
        public List<Product> Products { get; private set; }

        public Order(int number)
        {
            Products = new List<Product>();
            FillOrder();
            Id = number;
        }
        private void FillOrder()
        {
            bool isFull = false;

            while (!isFull)
            {
                Console.Clear();
                Console.WriteLine("1 - Добавить в заказ позицию\n2 - Удалить из заказа позицию\n3 - Подтвердить заказ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        AddPosition();
                        break;
                    case "2":
                        Console.Clear();

                        if (Products.Count == 0)
                        {
                            Console.WriteLine("Список продуктов пустой, удаление невозможно!");
                        }
                        else
                        {
                            Console.WriteLine("Введите номер позиции, которую вы хотите удалить");
                            int index = 0;
                            Products.RemoveAt(GetIndex(index));
                        }
                        break;
                    case "3":
                        Console.Clear();
                        isFull = true;
                        break;
                }
            }
        }
        private int GetIndex(int index)
        {
            bool isCorrect = false;

            while (!isCorrect)
            {
                if (Int32.TryParse(Console.ReadLine(), out int result) && result > 0 && result <= Products.Count)
                {
                    index = result;
                    isCorrect = true;
                }
            }
            return index - 1;
        }
        private void AddPosition()
        {
            Console.WriteLine("1 - Добавить еду\n2 - Добавить мебель\n3 - Добавить одежду\n4 - Добавить электронику");

            switch (Console.ReadLine())
            {
                case "1":
                    Products.Add(new Food());
                    break;
                case "2":
                    Products.Add(new Furniture());
                    break;
                case "3":
                    Products.Add(new Clothes());
                    break;
                case "4":
                    Products.Add(new Electronics());
                    break;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool isWorking = true;
            User user = CreateUser();

            Console.WriteLine("Сделайте первый заказ!");
            user.MakeOrder();

            while (isWorking)
            {
                Console.WriteLine("1 - Сделать новый заказ\n2 - Показать все ваши заказы\n3 - Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        user.MakeOrder();
                        break;
                    case "2":
                        user.ShowAllOrders();
                        break;
                    case "3":
                        isWorking = false;
                        break;
                }
            }
        }
        static private User CreateUser()
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            return new User(name);
        }
    }
}