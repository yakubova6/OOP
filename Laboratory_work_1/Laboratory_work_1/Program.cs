using System;
using System.Text;

namespace TypeConversionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПРЕОБРАЗОВАНИЯ ТИПОВ ===\n");

            // 1. Неявное преобразование (Implicit Conversion)
            Console.WriteLine("1. НЕЯВНОЕ ПРЕОБРАЗОВАНИЕ:");
            Console.WriteLine("---------------------------");

            // ПРИМЕРЫ С ПРОСТЫМИ ТИПАМИ
            int intValue = 253;
            double doubleValue = intValue; // Неявное преобразование int -> double
            Console.WriteLine($"int {intValue} -> double {doubleValue}");

            short shortValue = 67;
            long longValue = shortValue; // Неявное преобразование short -> long
            Console.WriteLine($"short {shortValue} -> long {longValue}");

            float floatValue = 3.14f;
            double doubleFromFloat = floatValue; // Неявное преобразование float -> double
            Console.WriteLine($"float {floatValue} -> double {doubleFromFloat}");

            // ПРИМЕРЫ С ССЫЛОЧНЫМИ ТИПАМИ
            string str = "Hello";
            object obj = str; // Неявное преобразование string -> object (восходящее)
            Console.WriteLine($"string '{str}' -> object (ссылочное преобразование)");

            MyDerivedClass derived = new MyDerivedClass();
            MyBaseClass baseRef = derived; // Неявное преобразование производного класса к базовому
            Console.WriteLine("MyDerivedClass -> MyBaseClass (восходящее преобразование)");

            // Таблица неявных преобразований
            /*
            ПРОСТЫЕ ТИПЫ:
            byte    -> short, ushort, int, uint, long, ulong, float, double, decimal
            short   -> int, long, float, double, decimal  
            int     -> long, float, double, decimal
            long    -> float, double, decimal
            float   -> double
            char    -> ushort, int, uint, long, ulong, float, double, decimal
            
            ССЫЛОЧНЫЕ ТИПЫ:
            производный класс -> базовый класс (восходящее)
            класс -> интерфейс 
            string -> object
            */

            // 2. Явное преобразование (Explicit Conversion)
            Console.WriteLine("\n2. ЯВНОЕ ПРЕОБРАЗОВАНИЕ:");
            Console.WriteLine("------------------------");

            // ПРИМЕРЫ С ПРОСТЫМИ ТИПАМИ
            double preciseValue = 123.456;
            int intFromDouble = (int)preciseValue; // Явное преобразование double -> int
            Console.WriteLine($"double {preciseValue} -> int {intFromDouble} (потеря точности!)");

            long bigValue = 1000000000;
            int intFromLong = (int)bigValue; // Явное преобразование long -> int
            Console.WriteLine($"long {bigValue} -> int {intFromLong}");

            // ПРИМЕРЫ С ССЫЛОЧНЫМИ ТИПАМИ
            object obj2 = "Test string";
            string str2 = (string)obj2; // Явное преобразование object -> string (нисходящее)
            Console.WriteLine($"object -> string: '{str2}'");


            // Таблица явных преобразований 
            /*
            ПРОСТЫЕ ТИПЫ:
            sbyte   -> byte, ushort, uint, ulong, char
            short   -> byte, sbyte, ushort, uint, ulong, char
            int     -> byte, sbyte, short, ushort, uint, ulong, char
            long    -> byte, sbyte, short, ushort, int, uint, ulong, char
            double  -> byte, sbyte, short, ushort, int, uint, long, ulong, char, decimal
            
            ССЫЛОЧНЫЕ ТИПЫ:
            базовый класс -> производный класс (нисходящее)
            интерфейс -> класс (явное приведение)
            object -> любой ссылочный тип
            Array -> конкретный тип массива
            */

            // 3. Безопасное приведение с помощью as и is
            Console.WriteLine("\n3. БЕЗОПАСНОЕ ПРИВЕДЕНИЕ (as/is):");
            Console.WriteLine("--------------------------------");

            object obj1 = "Hello World";
            object obj3 = 42;
            object obj4 = new MyDerivedClass();

            // Оператор is - проверка типа (возвращает bool)
            if (obj1 is string)
            {
                Console.WriteLine("obj1 является строкой");
                string strFromObj1 = (string)obj1; // Безопасное приведение после проверки (изменил имя переменной)
                Console.WriteLine($"Длина строки: {strFromObj1.Length}");
            }

            // Модифицированный is (проверка + приведение в одной операции):
            if (obj1 is string strObj)
            {
                Console.WriteLine($"Улучшенная проверка: obj1 = '{strObj}', длина = {strObj.Length}");
            }

            // Проверка с пользовательскими типами
            if (obj4 is MyDerivedClass)
            {
                Console.WriteLine("obj4 является MyDerivedClass");
            }

            // Оператор as - безопасное приведение (возвращает null при неудаче)
            string str1 = obj1 as string;
            if (str1 != null)
            {
                Console.WriteLine($"Приведение obj1 успешно: '{str1}'");
            }

            string str3 = obj3 as string; // Вернет null, т.к. int нельзя привести к string
            if (str3 == null)
            {
                Console.WriteLine("Приведение obj3 к string не удалось (вернулся null)");
            }

            // 4. Пользовательское преобразование (Custom Conversion)
            Console.WriteLine("\n4. ПОЛЬЗОВАТЕЛЬСКОЕ ПРЕОБРАЗОВАНИЕ:");
            Console.WriteLine("----------------------------------");

            Meters distanceInMeters = new Meters(2500);

            // Явное преобразование Meters -> Kilometers
            Kilometers km = (Kilometers)distanceInMeters;
            Console.WriteLine($"Явное преобразование: {distanceInMeters} = {km}");

            // Неявное преобразование Kilometers -> Meters
            Meters backToMeters = km;
            Console.WriteLine($"Неявное преобразование: {km} = {backToMeters}");

            // 5. Преобразование с помощью Convert, Parse, TryParse
            Console.WriteLine("\n5. КЛАСС CONVERT И МЕТОДЫ PARSE/TRYPARSE:");
            Console.WriteLine("------------------------------------------");

            string numberStr = "123";

            // Класс Convert - универсальное преобразование
            int convertedInt = Convert.ToInt32(numberStr);
            double convertedDouble = Convert.ToDouble(numberStr);
            Console.WriteLine($"Convert.ToInt32(\"{numberStr}\") = {convertedInt}");
            Console.WriteLine($"Convert.ToDouble(\"{numberStr}\") = {convertedDouble}");

            // Метод Parse - преобразование с исключением при ошибке
            try
            {
                int parsedInt = int.Parse(numberStr);
                Console.WriteLine($"int.Parse(\"{numberStr}\") = {parsedInt}");


                // Это выбросит исключение
                int invalidParse = int.Parse("abc");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка Parse: {ex.Message}");
            }

            // Метод TryParse - безопасное преобразование (возвращает bool)
            string[] testValues = { "456", "abc", "-789", "123.45" };

            foreach (string value in testValues)
            {
                if (int.TryParse(value, out int tryParseResult))
                {
                    Console.WriteLine($"TryParse успешен для '{value}': {tryParseResult}");
                }
                else
                {
                    Console.WriteLine($"TryParse не удался для '{value}'");
                }
            }
        }
    }

    // Базовый и производный классы для демонстрации преобразований ссылочных типов
    public class MyBaseClass
    {
        public string BaseProperty { get; set; } = "Base Value";
    }

    public class MyDerivedClass : MyBaseClass
    {
        public string DerivedProperty { get; set; } = "Derived Value";
    }

    // Классы для пользовательского преобразования
    public class Meters
    {
        public double Value { get; set; }

        public Meters(double value)
        {
            Value = value;
        }

        // Неявное преобразование Kilometers -> Meters
        // Используется когда преобразование безопасное и не теряет данные
        public static implicit operator Meters(Kilometers km)
        {
            return new Meters(km.Value * 1000);
        }

        // Явное преобразование Meters -> Kilometers
        // Используется когда преобразование может потерять точность
        public static explicit operator Kilometers(Meters meters)
        {
            return new Kilometers(meters.Value / 1000);
        }

        public override string ToString() => $"{Value} м";
    }

    public class Kilometers
    {
        public double Value { get; set; }

        public Kilometers(double value)
        {
            Value = value;
        }

        public override string ToString() => $"{Value} км";
    }
}