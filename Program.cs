using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spz_lb1
{
    public struct mwsc
    {
        public int milk, water, sugar, coffee;
    }

    public enum drinks
    {
        espresso = 20,
        latte = 35,
        cappuccino = 35,
        lungo = 20,
        flatwhite = 45
    }

    class coffee_machine
    {
        private readonly string name;
        private const int max_sugar = 500, max_milk = 10000, max_coffee = 5000, max_water= 20000;
        private static uint cash;
        public mwsc component = new mwsc();
        public coffee_machine() { name = "Name"; }
       
        public void display_info()
        {
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"\tWater: {component.water}\tMilk:   {component.milk}");
            Console.WriteLine($"\tSugar: {component.sugar}\tCoffee: {component.coffee}");
        }

        private bool consum(int m, int w, int s, int c)
        {
            if (m > component.milk || w > component.water || s > component.sugar || c > component.coffee)
                return true;
            else
            {
                component.coffee -= c;
                component.milk -= m;
                component.sugar -= s;
                component.water -= w;
            }
            return false;
        }

        public int cashin(int [] num, int money)
        {
            int sum = 0;
            for (var i = 0; i < num.Length; i++)
            {
                if (num[i] > 5)
                    return 1;
                else
                {
                    switch (num[i])
                    {
                        case 1:
                            sum += (int)drinks.espresso;
                            break;
                        case 2:
                            sum += (int)drinks.latte;
                            break;
                        case 3:
                            sum += (int)drinks.cappuccino;
                            break;
                        case 4:
                            sum += (int)drinks.lungo;
                            break;
                        case 5:
                            sum += (int)drinks.flatwhite;
                            break;
                    }
                }
                if (sum > money)
                    return 2;
            }
            return 0;
        }

        public void refill(int m, int w, int s, int c)
        {
            component.milk = component.milk + m > max_milk ? max_milk : component.milk + m;
            component.water = component.water + w > max_water ? max_water : component.water + w;
            component.sugar = component.sugar + s > max_sugar ? max_sugar : component.sugar + s;
            component.coffee = component.coffee + c > max_coffee ? max_coffee : component.coffee + c;
        }

        public void cashout()
        {
            Console.WriteLine($"Инкассировано {cash} грн");
            cash = 0;
        }

        public bool check()
        {
            if (component.milk < 500 || component.water < 500 || component.coffee < 80 || component.sugar < 50)
                return false;
            return true;
        }

        public bool cook(int [] num)
        {
            foreach (var n in num)
            {
                switch (n)
                {
                    case 1:
                        cash += (uint)drinks.espresso;
                        if (consum(0, 30, 0, 8)) return true;
                        break;
                    case 2:
                        cash += (uint)drinks.latte;
                        if (consum(100, 30, 10, 8)) return true;
                        break;
                    case 3:
                        cash += (uint)drinks.cappuccino;
                        if (consum(100, 30, 0, 8)) return true;
                        break;
                    case 4:
                        cash += (uint)drinks.lungo;
                        if(consum(0, 60, 0, 8)) return true;
                        component.coffee -= 8;
                        break;
                    case 5:
                        cash += (uint)drinks.flatwhite;
                        if(consum(70, 60, 0, 16)) return true;
                        break;
                }
            }
            return false;
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            int money;
            var machine = new coffee_machine();
            var num = new int[] { };
            machine.refill(1000, 1000, 100, 10000000);
            machine.display_info();
            while (true)
            {
                Console.WriteLine("Нажмите любую клавишу чтобы начать");
                if ((Console.ReadKey().Key) == ConsoleKey.Escape)
                    machine.cashout();
                if (!machine.check())
                {
                    Console.WriteLine("Недостаточно ингридиентов! Приготовление напитка невозможно!");
                    Console.ReadKey();
                    return 0;
                }
                Console.WriteLine("\t1.Espresso   20");
                Console.WriteLine("\t2.Latte      35");
                Console.WriteLine("\t3.Cappuccino 35");
                Console.WriteLine("\t4.Lungo      20");
                Console.WriteLine("\t5.Flatwhite  45");
                Console.WriteLine("Внесите купюру");
                if (int.TryParse(Console.ReadLine(), out money))
                {
                    Console.WriteLine("Выберите напиток");
                    try
                    {
                        num = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
                    }
                    catch
                    {
                        Console.WriteLine("Данные введены неверно!");
                        continue;
                    }
                    if((machine.cashin(num, money)) == 1)
                        Console.WriteLine("Неверный номер напитка!");
                    else if((machine.cashin(num, money)) == 2)
                        Console.WriteLine("Недостаточно средств!");
                    else
                    {
                        if (machine.cook(num))
                            Console.WriteLine("Недостаточно ингридиентов!");
                        else
                        {
                            Console.WriteLine("Ваш напиток готов");
                        }
                        machine.display_info();
                    }
                }
                else
                    Console.WriteLine("Невалидная купюра, повторите ввод!");
            }
        }
    }
}
