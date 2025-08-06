using System;
using System.Diagnostics;
using System.Text;

namespace un7cod8
{
    public class Program
    {
        //运行标识 当为false时退出命令输入循环
        internal static bool running = true;

        static void Main(string[] args)
        {
            //string str1 = Encode("Hello你好");
            //Console.WriteLine(str1);
            //string str2 = Decode("8778777 8877878 8878877 8878877 8878888 88777778778888 88888788788778");
            //Console.WriteLine(str2);

            InitText();
            CommandHandler.AssignCommands();
            //命令处理
            while(running)
            {
                Console.Write(" >");
                string command = Console.ReadLine();
                CommandHandler.HandleCommand(command);
            }
        }

        /// <summary>
        /// 显示初始化文本
        /// </summary>
        static void InitText()
        {
            Console.WriteLine("           [UN7COD8 - 文本转换器]");
            Console.WriteLine("     Source: [Github:YangQi0408] https://github.com/YangQi0408/un7cod8 - AGPL-3.0\n");
            Console.WriteLine("     Current branch: 'objectivec' by AstarLC");
            Console.WriteLine("     Enter 'help' for help.\n");
        }

        /// <summary>
        /// 编码un7cod8
        /// </summary>
        /// <param name="source">源文本</param>
        /// <returns>编码后的文本</returns>
        internal static string Encode(string source)
        {
            //拆分字符
            char[] chars = source.ToCharArray();
            string result = "";
            //处理字符
            foreach (char c in chars)
            {
                /*
                //获取每个字符的unicode对应的byte
                byte[] bytes = Encoding.Unicode.GetBytes(c.ToString());
                //固定byte编码
                string binaryStr1 = Convert.ToString(bytes[0], 2);
                string encodedStr1 = binaryStr1.Replace("0", "7").Replace("1", "8");
                string encodedStr2 = "";
                //非固定byte编码(中文等)
                if (bytes[1] != 0)
                {
                    string binaryStr2 = Convert.ToString(bytes[1], 2);
                    encodedStr2 = binaryStr2.Replace("0", "7").Replace("1", "8");
                }
                //由于转换的时候得到了两个bytes,所以要看情况填补0来恢复原数据
                string temp = bytes[1] == 0 ? encodedStr1 + " " : encodedStr2 + "x" + encodedStr1 + " ";
                if(temp.Length - 2 <= 15 && bytes[1] != 0)
                {
                    int fixes = 15 - (temp.Length - 2);
                    if (fixes == 0)
                    {
                        temp = temp.Replace("x", "7");
                    }
                    else
                    {
                        string replaceStr = "";
                        for(int i = 0; i < fixes; i++)
                        {
                            replaceStr += "7";
                        }
                        temp = temp.Replace("x", replaceStr);
                    }
                }
                //拼合文本
                result += temp;
                //Console.WriteLine(((byte)c).ToString("X4"));
                */
                int unicode = c;
                //Console.WriteLine(unicode);
                string temp = Convert.ToString(unicode, 2).Replace("0", "7").Replace("1", "8");
                result += temp + " ";
            }
            return result;
        }

        /// <summary>
        /// 解码un7cod8
        /// </summary>
        /// <param name="source">源文本</param>
        /// <returns>解码后的文本</returns>
        internal static string Decode(string source)
        {
            //还原为二进制unicode(byte)
            string origin = source.Replace("7", "0").Replace("8", "1");
            //拆分
            string[] charsInBin = origin.Split(' ');
            string result = "";
            //处理解码
            foreach(string bin in charsInBin)
            {
                try
                {
                    result += (char)Convert.ToInt32(bin, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("解码失败: " + ex.Message);
                }
                /*
                byte[] bytes = new byte[2];

                //当字符量大于9时视作两个byte代表的字符
                if (bin.Length >= 9)
                {
                    //还原初始byte
                    string[] substrs = bin.Split('x');
                    bytes[0] = (byte)Convert.ToInt32(substrs[1], 2);
                    bytes[1] = (byte)Convert.ToInt32(substrs[0], 2);
                }
                else
                {
                    //还原初始byte
                    bytes[0] = (byte)Convert.ToInt32(bin, 2);
                    bytes[1] = 0;
                }
                //unicode解码与拼合
                result += Encoding.Unicode.GetString(bytes);
                */
            }
            return result;
        }
    }
}
