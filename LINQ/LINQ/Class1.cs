using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LINQ
{

    static class StringExtensions
    {
        public static string ToNameCase(this string source) => source.ToLower().Remove(0, 1)
                    .Insert(0, source.Substring(0, 1).ToUpper());

    }

    public class Date
    {
        private ushort day, month;
        public ushort Day
        {
            get => day;
            set
            {
                if (value > 31)
                {
                    day = 31;
                }
                else
                {
                    day = value;
                }
            }
        }
        public ushort Month
        {
            get => month;
            set
            {
                if (value > 12)
                {
                    month = 12;
                }
                else
                {
                    month = value;
                }
            }
        }
        public int Year { get; set; }


        public Date(string date)
        {
            string[] tokens = date.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 3)
            {
                Debug.WriteLine("парсинг даты сломался");
            }
            else
            {
                Day = ushort.Parse(tokens[0]);
                Month = ushort.Parse(tokens[1]);
                Year = int.Parse(tokens[2]);

            }

        }

        public Date(ushort day, ushort month, int year) => (Day, Month, Year) = (day, month, year);

 


        public static implicit operator Date(string s) => new Date(s);

        public override string ToString() => $"{Day}.{Month}.{Year}";

        public override bool Equals(object o) => (o is Date date) &&
            (date.Day, date.Month, date.Year) == (Day, Month, Year);
       
        public override int GetHashCode() => HashCode.Combine(Day, Month, Year);
    }

    public class Person : IComparable<Person>
    {
        private string surname;
        private string name;
        private string patronymic;


        public string Surname
        {
            get => surname;
            set => surname = value.ToNameCase();
        }

        public string Name
        {
            get => name;
            set => name = value.ToNameCase();
        }

        public string Patronymic
        {
            get => patronymic;
            set => patronymic = value.ToNameCase();
        }

        public string FullName => $"{Surname} {Name} {Patronymic}";

        public ushort Age =>
              (ushort)((DateTime.Now.Year - Birthday.Year - 1) + (((DateTime.Now.Month > Birthday.Month)
            || ((DateTime.Now.Month == Birthday.Month) && (DateTime.Now.Day >= Birthday.Day))) ? 1 : 0));

        public Date Birthday { get; protected set; }


        public Person(string surname, string name, string patronymic, Date birthday)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Birthday = birthday;
        }

        //в этом языке не любят copy ctorы поэтому пускай будет protected)))
        protected Person(Person p) : this(p.Surname, p.Name, p.Patronymic, p.Birthday)
        {
        }

        public Person(string personaldata)
        {
            string[] pd = personaldata.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (pd.Length != 4)
            {
                Debug.WriteLine("парсинг личных данных сломался");
            }
            else
            {
                Surname = pd[0];
                Name = pd[1];
                Patronymic = pd[2];
                Birthday = pd[3];

            }

        }


        public override string ToString() => FullName + $" {Birthday}";

        public override bool Equals(object o)
        {
            if ((o == null) || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                var p = (Person)o;
                return (p.Name == Name) && (p.Patronymic == Patronymic) && (p.Surname == Surname) && p.Birthday.Equals(Birthday);
            }
        }

        public override int GetHashCode() => HashCode.Combine(Surname, Name, Patronymic, Birthday);

        public int CompareTo(Person other) => Age.CompareTo(other.Age);
       
    }

    public class Student : Person
    {
        static string infoTemplate;
        static int currentCount;
        public static string Info
          => string.Format(infoTemplate, typeof(Student).Name, currentCount);

        public readonly int id;



        private string phoneNumber;



        static Student()
        {
            currentCount = 0;
            infoTemplate = "Класс {0}, на данный момент насчитывающий {1} объектов";
        }

        public override string ToString() => base.ToString() + $" {Faculty} {Course} {Group}";

        //  Адрес,  Телефон, Факультет, Курс, Группа.


        public string Address
        {
            get; set;
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                var toSet = value.Trim();
                if (toSet.Length != 13)
                {
                    Debug.WriteLine("номер должен быть 13 символов");
                }
                else
                {
                    phoneNumber = toSet;
                }

            }
        }

        public string Faculty
        {
            get; set;
        }
        public int Course
        {
            get; set;
        }

        public int Group
        {
            get; set;
        }

        public override bool Equals(object o)
        {

            if ((o == null) || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                var s = (Student)o;
                return base.Equals(s) && (s.Address == Address) && (s.PhoneNumber == PhoneNumber) && (s.Faculty == Faculty) && (s.Course == Course) && (s.Group == Group);
            }
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Address);
            hash.Add(PhoneNumber);
            hash.Add(Faculty);
            hash.Add(Course);
            hash.Add(Group);
            return hash.ToHashCode();
        }

        private Student(Person p, string address, string phonenumber, string faculty, int course = 1, int group = 1) : base(p)
        {
            Address = address;
            PhoneNumber = phonenumber;
            Faculty = faculty;
            Course = course;
            Group = group;
            currentCount++;
            id = Math.Abs(GetHashCode());
        }
        //аргументы по умолчанию есть! приватный конструктор есть!
        public Student(string personaldata, string address, string phonenumber, string faculty, int course, int group) :
            this(new Person(personaldata), address, phonenumber, faculty, course, group)
        {
        }

        public Student() : this(new Person("Неизвестно Неизвестно Неизвестно 01.01.1970"), "Неизвестно", "Неизвестно", "Неизвестно")
        {
        }

        ~Student() => currentCount--;

    }
}
