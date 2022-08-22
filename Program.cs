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
        public string Address;

    }
    class Courier
    {
        public CouriersNames Name;
        Random rand = new Random();

        public Courier()
        {
            Name = (CouriersNames)rand.Next(0, 2);
        }
    }

    class HomeDelivery : Delivery
    {
        public Courier Courier;
        public HomeDelivery()
        {
            Console.WriteLine("Введите ваш адрес");
            Address = Console.ReadLine();
            Courier = new Courier();
        }
    }

    class PickPointDelivery : Delivery
    {
        public PickPointDelivery()
        {
            Address = "PickPoint";
        }
    }

    class ShopDelivery : Delivery
    {
        public ShopDelivery()
        {
            Address = "Shop";
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
        public string Name;
        public List<Order<HomeDelivery>> HomeOrders = new List<Order<HomeDelivery>>();
        public List<Order<PickPointDelivery>> PointOrders = new List<Order<PickPointDelivery>>();
        public List<Order<ShopDelivery>> ShopOrders = new List<Order<ShopDelivery>>();
        public int OrdersCount;

        public User(string name)
        {
            OrdersCount = 0;
            Name = name;
            HomeOrders = new List<Order<HomeDelivery>>();
            PointOrders = new List<Order<PickPointDelivery>>();
            ShopOrders = new List<Order<ShopDelivery>>();
        }
        public void AddOrder<TDelivery>(List<Order<TDelivery>> listOrders) where TDelivery : Delivery
        {
            Console.Clear();
            Order<TDelivery> order = new Order<TDelivery>(OrdersCount + 1);
            listOrders.Add(order);
            OrdersCount++;
        }
        public void ShowOrders<TDelivery>(List<Order<TDelivery>> listOrders) where TDelivery : Delivery
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
    }
    class Order<TDelivery> where TDelivery :  Delivery
    {
        public TDelivery Delivery;
        public int Id;
        public List<Product> Products;

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
            HomeDelivery homeDelivery = new HomeDelivery();

            while (isWorking)
            {
                Console.WriteLine($"Доставка по адресу: {homeDelivery.Address}");
                Console.WriteLine("1 - Сделать новый заказ\n2 - Показать все ваши заказы\n3 - Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Выберите тип доставки:\n1 - На дом\n2 - В пункт выдачи\n3 - В магазин");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                user.AddOrder(user.HomeOrders);
                                user.HomeOrders[user.HomeOrders.Count - 1].Delivery = new HomeDelivery();
                                break;
                            case "2":
                                user.AddOrder(user.PointOrders);
                                user.PointOrders[user.PointOrders.Count - 1].Delivery = new PickPointDelivery();
                                break;
                            case "3":
                                user.AddOrder(user.ShopOrders);
                                user.ShopOrders[user.ShopOrders.Count - 1].Delivery = new ShopDelivery();
                                break;
                        }
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Заказы на дом:");
                        user.ShowOrders(user.HomeOrders);
                        Console.WriteLine("Заказы в пункты выдачи:");
                        user.ShowOrders(user.PointOrders);
                        Console.WriteLine("Заказы в магазин:");
                        user.ShowOrders(user.ShopOrders);
                        break;
                    case "3":
                        isWorking = false;
                        break;
                }
            }
        }
        static public User CreateUser()
        {
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            return new User(name);
        }
    }
}