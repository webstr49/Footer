using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footer
{
    class FileOperation
    {
        private int sizeProperty = 0;
        private bool contains = false;
        private string[] footer;
        private string view = "";
        private string path = "";
        private const string Header = "[Company]";

        public string GetProperties(string fileName)
        {
            if (File.Exists(fileName))
            {

                var fileInfo = new FileInfo(fileName);

                string chars;

                using (var stream = File.OpenRead(fileName))
                {
                    if (fileInfo.Length >= 1024)
                    {
                        stream.Seek(fileInfo.Length - 1024, SeekOrigin.Begin);
                        using (var textReader = new StreamReader(stream))
                        {
                            chars = textReader.ReadToEnd();

                        }

                    }

                    else
                    {
                        stream.Seek(fileInfo.Length - fileInfo.Length, SeekOrigin.Begin);
                        using (var textReader = new StreamReader(stream))
                        {
                            chars = textReader.ReadToEnd();

                        }

                    }
                    if (chars.Contains(Header) && chars != null)
                    {
                        string properties = chars.Substring(chars.IndexOf(Header));
                        sizeProperty = properties.Length;
                        contains = true;

                        return Sort(properties);

                    }
                    else
                    {
                        return OutputString.FooterNotExist;
                    }
                }

            }
            return OutputString.FileNotFound;

        }

        private string Sort(string input)
        {

            view = "";
            footer = input.Replace("\r", "").Split('\n');
            foreach (string property in footer)
            {
                if (property == string.Empty) continue;
                view += property + " ";

            }
            return view;
        }

        public string Add(string fileName, string property)
        {

            string duplicit = property.Split('=')[0];

            if (footer != null)
            {
                foreach (string properties in footer)
                {
                    if (properties.Split('=')[0].ToLower() == duplicit.ToLower())
                    {
                        return OutputString.Duplicit;
                    }
                }
            }

            if (sizeProperty + property.Length + 1 <= 1024)
            {
                using (StreamWriter sw = new StreamWriter(fileName, true))
                {

                    if (contains == false)
                    {
                        sw.WriteLine(Header+"\n");
                    }
                    if (property != string.Empty)
                    {
                        sw.WriteLine(property + "\n");
                    }

                }

            }
            else
            {
                GetProperties(fileName);
                return OutputString.LargeOut;
            }
            GetProperties(fileName);
            return null;
        }

        public bool Remove(string fileName, string property)
        {
            if (File.Exists(fileName) && sizeProperty > 0 && contains)
            {
                string condition = property.Split('=')[0];
                int count = 0;
                int index = 0;
                foreach (string properties in footer)
                {
                    if (properties.Split('=')[0].ToLower() == condition.ToLower())
                    {
                        footer[index] = "";
                    }
                    if (properties != string.Empty)
                    {
                        count++;
                    }
                    index++;
                }

                Delete(fileName);
                if (count - 1 > 1)
                {
                    Write(fileName);
                }

            }

            GetProperties(fileName);
            return false;

        }

        public void Edit(string fileName, string property)
        {
            if (sizeProperty > 0)
            {

                string condition = property.Split('=')[0];
                int index = 0;
                foreach (string properties in footer)
                {
                    if (properties.Split('=')[0].ToLower() == condition.ToLower())
                    {
                        footer[index] = property;
                    }
                    index++;

                }
                Delete(fileName);
                Write(fileName);
                GetProperties(fileName);
            }
        }

        private void Write(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                foreach (string properties in footer)
                {
                    if (properties == string.Empty) continue;
                    sw.WriteLine(properties + "\n");
                }

            }
            GetProperties(fileName);
        }
        private void Delete(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                fs.SetLength(fs.Length - sizeProperty);
                fs.Close();
                contains = false;

            }
        }

        public string GetPath(string fileName)
        {
            string configFile = "";
            try
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Footer");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                Console.WriteLine(OutputString.Repository, path);
            }
            if (File.Exists(Path.Combine(path, "config.txt")))
            {
                try
                {
                    if (fileName == string.Empty)
                    {


                        string path2 = Path.Combine(path, "config.txt");
                        using (StreamReader sr = new StreamReader(path2))
                        {
                            string s;
                            while ((s = sr.ReadLine()) != null)
                            {
                                configFile += s;
                            }

                        }
                    }
                    else
                    {
                        if (File.Exists(fileName))
                        {

                            string path2 = Path.Combine(path, "config.txt");

                            using (StreamWriter sw = new StreamWriter(path2))
                            {
                                sw.WriteLine(fileName);
                                sw.Flush();
                            }

                        }
                        else
                        {
                            Console.WriteLine(OutputString.Path);
                        }


                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
                return configFile;
            }
            else
            {
                try
                {
                    string path2 = Path.Combine(path, "config.txt");

                    using (StreamWriter sw = new StreamWriter(path2))
                    {
                        sw.WriteLine(fileName);
                        sw.Flush();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
                return string.Empty;
            }
        }
    }
}
