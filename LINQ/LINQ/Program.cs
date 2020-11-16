 
using System;
using System.Globalization;
using System.Linq;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var months = new[] {"January", "February","March","April",
                    "May","June","July","August","September","October","November","December"}.OrderBy(el => random.Next()).ToArray();


            //1.1
            var n = 7;
            var monthQuery = months.Where(month => month.Length == n);



            foreach (var month in monthQuery)
            {
                Console.WriteLine(month);
            }

            //1.2

            Predicate<int> isWinterMonth = number => number == 1 || number == 2 || number == 12;

            Predicate<int> isSummerMonth = number => number == 6 || number == 7 || number == 8;

            var seasonQuery = from month in months
                              let monthNumber = Array.IndexOf(CultureInfo.InvariantCulture.DateTimeFormat.MonthNames, month) + 1
                              where isWinterMonth(monthNumber) || isSummerMonth(monthNumber)
                              select month;

            foreach (var month in seasonQuery)
            {
                Console.WriteLine(month);
            }

            //1.3


            var monthUQuery = months.Where(month => month.Length >= 4 && month.Contains('u'));

            foreach (var month in monthUQuery)
            {
                Console.WriteLine(month);
            }


            //2
            var students = new[]
            {
                new Student("тумаш станислав игоревич 22.08.2001", "улица блаблабла", "+375255377984", "ФИТ", 2, 1),
                new Student("савельев талан михайлович 03.07.1998", "улица1 блаблабла", "+375445938865", "ХТиТ", 2, 3),
                new Student("стельмаков константин ЕВГЕНЬЕВИЧ 14.06.2001","улица2 блаблабла", "+375335785843", "ФИТ", 2, 4),
                new Student("василевич владимир павлович 21.03.2000", "улица3 блаблабла", "+375255377994", "ФИТ", 2, 3),
                new Student("удовыдченкова анастасия александровна 12.01.2001", "улица4 блаблабла", "+375334845667", "ФИТ", 2, 9),
                new Student("карпенкин         дмитрий анатольевич 28.04.1999", "улица5 блаблабла", "+375257359612", "ФИТ", 2, 4),
                new Student("дежемесов александр егорович 29.12.2001", "улица6 блаблабла", "+375335785968", "ФИТ", 2, 3),
                new Student("челик какой-то нормальный 9.11.1996", "улица7 блаблабла", "+375335785968", "ФИТ", 2, 3),
                new Student("пулатов александр егорович 29.12.2001", "улица8 блаблабла", "+375335987777", "ФИТ", 2, 2),
                new Student("пулатов михаил егорович 29.12.2001", "улица9 блаблабла", "+375335537769", "ФИТ", 1, 3),
                new Student("липенко дмитрий павлович 220.10.2000", "улица10 блаблабла", "+375257359612", "ХТиТ", 2, 4),
            };


            //3.1
            var faculty = "фит";
            var studentFacultyQuery = students
                .Where(student => string.Equals(student.Faculty, faculty, StringComparison.OrdinalIgnoreCase))
                .OrderBy(student => student.FullName.First());

            foreach (var student in studentFacultyQuery)
            {
                Console.WriteLine(student);
            }

            //3.2

            var group = 3;

            var studentGroupFacultyQuery = students
              .Where(student => string.Equals(student.Faculty, faculty, StringComparison.OrdinalIgnoreCase) && student.Group == group);

            foreach (var student in studentGroupFacultyQuery)
            {
                Console.WriteLine(student);
            }

            //3.3

            var youngestStudent = students.Min();

            Console.WriteLine(youngestStudent);

            //3.4

            var studentGroupQuery = students
                .Where(student => student.Group == group)
                .OrderBy(student => student.Surname.First());
 
            foreach (var student in studentGroupQuery)
            {
                Console.WriteLine(student);
            }

            //3.5
            var name = "дмитрий";
            var firstWithName = students.First(student => string.Equals(student.Name, name, StringComparison.OrdinalIgnoreCase));

            Console.WriteLine(firstWithName);









            //4-5
            //минимальный возраст в каждой группе 2 курса по убыванию возраста

            var groupAge =
                from GA in
                    from student in students
                    where student.Course == 2
                    group student by student.Group into SG

                    select new
                    {
                        Group = SG.Key,
                        Age = SG.Min(student => student.Age)
                    }
                orderby GA.Age descending
                select GA;

            //является ли в хотя бы 1 группе 19 минимальным возрастом
            bool isAny = groupAge.Any(GA => GA.Age == 17);

            //пары студент минимальный возраст в его группе
            var studentGroupAge = (from student in students
                                   join GA in groupAge on student.Group equals GA.Group
                                   select new
                                   {
                                       Student = student,
                                       GA.Age
                                   }).Skip(2); //Skip чтоб было

            
           
            
          



                               
        }
    }

}