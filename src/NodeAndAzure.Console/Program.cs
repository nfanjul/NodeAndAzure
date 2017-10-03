using System;
using System.Threading.Tasks;

namespace NodeAndAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "NodeAndAzure - DEMO";
            while (true)
            {
                Console.Write("1º) Llamada a API" + "\n" + "2º) Llamada API paralela" + "\n" + "3º) Subir archivo Storage" + "\n" + "4º) Almacenar valor en Redis" + "\n" + "5º) Leer valor de Redis" + "\n" + "6º) Get SQL" + "\n");
                Console.Write("Seleccione una opción: ");
                string menu = Console.ReadLine();
                int switch_menu = int.Parse(menu);
                Console.WriteLine("-------------------------- INICIO! --------------------------");
                switch (switch_menu)
                {
                    case 1:
                        For();
                        break;
                    case 2:
                        ParallelFor();
                        break;
                    case 3:
                        Console.Write("Introduzca el nombre de la imagen: ");
                        CreateStorage(Console.ReadLine());
                        break;
                    case 4:
                        Console.Write("Introduzca un texto: ");
                        SetRedis(Console.ReadLine());
                        break;
                    case 5:
                        GetRedis();
                        break;
                    case 6:
                        GetSQL();
                        break;
                }
                Console.WriteLine("-------------------------- FIN! --------------------------");
            }
        }

        private static void For()
        {
            var rest = new RESTClient("<API_URL>", "");
            for (int i = 1; i <= 30; i++)
            {
                var result = rest.Get();
                Console.WriteLine(result + " For: " + i);
            }
        }

        private static void ParallelFor()
        {
            Parallel.For(1, 50, i =>
            {
                var rest = new RESTClient("<API_URL>", "");
                var result = rest.Get();
                Console.WriteLine(result + " ParallelFor: " + i);
            });
        }

        private static void CreateStorage(string path)
        {
            var image = $"C:\\Users\\xxxxx\\Pictures\\Camera Roll\\{path}";
            var result = RESTClient.PostImage(image);
            Console.WriteLine(result);
        }

        private static void SetRedis(string text)
        {
            var result = RESTClient.Post(text);
            Console.WriteLine(result);
        }

        private static void GetRedis()
        {
            var rest = new RESTClient("<API_URL>", "getredis");
            var result = rest.Get();
            Console.WriteLine(result);
        }

        private static void GetSQL()
        {
            var rest = new RESTClient("<API_URL>", "getsql");
            var result = rest.Get();
            Console.WriteLine(result);
        }

    }
}
