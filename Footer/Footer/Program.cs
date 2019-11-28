using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footer
{
    class Program
    {
        static void Main(string[] args)
        {
            string viewProperties = "";
            string fileName = "";
            FileOperation fileOperation = new FileOperation();


            if (!(args.Length < 1))
            {

                if (!(args.Length < 2))
                {


                    if (args[0].ToLower() == "add" && args[1].Contains('='))
                    {
                        string property = args[1].Split('=')[0];
                        string value = args[1].Substring(args[1].IndexOf("=") + 1);
                        if (property == string.Empty || value == string.Empty)
                        {
                            Console.WriteLine("Nesprávně zadána vlastnost \nProperty=value");
                            return;
                        }
                        fileName = fileOperation.GetPath("");
                        fileOperation.GetProperties(fileName);
                        if (fileName != string.Empty)
                        {
                            string info = fileOperation.Add(fileName, args[1]);
                            viewProperties = fileOperation.GetProperties(fileName);
                            Console.WriteLine(info);
                            Console.WriteLine(viewProperties);
                        }

                    }

                    else if (args[0].ToLower() == "edit" && args[1].Contains('='))
                    {
                        string property = args[1].Split('=')[0];
                        string value = args[1].Substring(args[1].IndexOf("=") + 1);
                        if (property == string.Empty || value == string.Empty)
                        {
                            Console.WriteLine("Nesprávně zadána vlastnost \nProperty=value");
                            return;
                        }
                        fileName = fileOperation.GetPath("");
                        fileOperation.GetProperties(fileName);
                        if (fileName != string.Empty)
                        {
                            fileOperation.Edit(fileName, args[1]);
                            viewProperties = fileOperation.GetProperties(fileName);
                            Console.WriteLine(viewProperties);
                        }
                    }

                    else if (args[0].ToLower() == "remove" && args[1] != string.Empty)
                    {
                        fileName = fileOperation.GetPath("");
                        if (fileName != string.Empty)
                        {
                            fileOperation.GetProperties(fileName);
                            bool info = fileOperation.Remove(fileName, args[1]);
                            viewProperties = fileOperation.GetProperties(fileName);
                            if (info == false)
                            {
                                Console.WriteLine("Odebrání se nezdařilo");
                            }
                            Console.WriteLine(viewProperties);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Nesprávny tvar příkazu\n footer.exe modifikator property=value");
                    }
                }

                else
                {
                    Console.WriteLine("Zadejte modifikat property=value");
                }
            }
            else
            {
                Console.WriteLine("Zadejte cest k souboru:");
                string path = Console.ReadLine();
                fileName = fileOperation.GetPath(path);
            }

        }
    }
}
