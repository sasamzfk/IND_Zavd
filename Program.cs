using System;
using System.Text;


namespace ConsoleApp21
{
    public class Child
    {
        private int childNumber;

        public int ChildNumber
        {
            get { return childNumber; }
        }

        public Child(int number)
        {
            this.childNumber = number;
        }
    }

    public class PlaySchool
    {
        private int capacity;
        private int currentCount;

        public event EventHandler NotPlaces;

        public PlaySchool(int places)
        {
            this.capacity = places;
            this.currentCount = 0;
            Console.WriteLine($"--- Дитячий садок відкрито. Кількість місць: {capacity} ---");
        }


        public void PushChild(Child child)
        {
            if (currentCount < capacity)
            {
                // Місця є
                currentCount++;
                Console.WriteLine($"Дитина <{child.ChildNumber}> зарахована. (Зайнято {currentCount} з {capacity})");
            }
            else
            {
                // Місць нема
                Console.WriteLine($"--- Спроба зарахувати дитину <{child.ChildNumber}>... ---");
                OnNotPlaces();
            }
        }

        protected virtual void OnNotPlaces()
        {
            // Перевірка
            if (NotPlaces != null)
            {
                NotPlaces(this, EventArgs.Empty);
            }
        }
    }

    public class Manageress
    {
       
        public event EventHandler Zapys;

        public void Queue(object sender, EventArgs e)
        {
            Console.WriteLine("ЗАВІДУВАЧ: Місць немає! Пропоную стати в чергу.");

            OnZapys();
        }

        protected virtual void OnZapys()
        {
            Zapys?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Department
    {
        public void Place(object sender, EventArgs e)
        {
            Console.WriteLine("РАЙОНО: Інші діти записуються в чергу.");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Введіть кількість місць у дитячому садку: ");
            int capacity = 0;
            while (!int.TryParse(Console.ReadLine(), out capacity) || capacity <= 0)
            {
                Console.Write("Некоректне введення. Введіть додатнє число: ");
            }

            PlaySchool kindergarten = new PlaySchool(capacity);
            Manageress manager = new Manageress();
            Department department = new Department();

            //  Queue
            kindergarten.NotPlaces += manager.Queue;

            // Метод Place
            manager.Zapys += department.Place;

            Console.WriteLine("\n--- Починаємо зарахування дітей ---");
            int childId = 1;

            for (int i = 0; i < capacity + 1; i++)
            {
                Child newChild = new Child(childId);

                kindergarten.PushChild(newChild);

                childId++;
                Console.WriteLine(); 
            }

            Console.WriteLine("--- Зарахування завершено ---");
        }
    }
}