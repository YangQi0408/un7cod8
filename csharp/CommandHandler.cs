using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace un7cod8
{
    /// <summary>
    /// 一个简单的命令解释器
    /// </summary>
    public class CommandHandler
    {
        //allow "-pattern <space*n> arg"
        public readonly static string commandArgMatchPattern = "-[a-zA-z](\\s*)([^\"].{1,}?(\\s|$)|\".{1,}?\")";
        //not allow: -[a-zA-z]\s([^"]\S{1,}?(\s|$)|"\S{1,}?")

        static Dictionary<string, Action<List<Argument>>> commandDir = new Dictionary<string, Action<List<Argument>>>();

        /// <summary>
        /// 批量注册所有命令
        /// </summary>
        public static void AssignCommands()
        {
            AssignCommand("help", (args) => {
                if (args.Count == 0)
                {
                    Console.WriteLine(
                        "EXIT     退出程序。\n" +
                        "HELP     获取命令帮助。\n" +
                        "ENCODE   编码78\n" +
                        "  -S     源字符串\n" + 
                        "DECODE   解码78\n" +
                        "  -S     源字符串");
                }
                else
                {
                    Console.WriteLine("参数数量错误");
                }
            });
            AssignCommand("exit", (args) => {
                if (args.Count == 0)
                {
                    Console.WriteLine("正在退出程序...");
                    Program.running = false;
                }
                else
                {
                    Console.WriteLine("参数数量错误");
                }
            });
            AssignCommand("encode", (args) => {
                if (args.Count == 1)
                {
                    Argument stringArg = args.Find(x => x.pattern == "S");
                    if(stringArg != null)
                    {
                        Console.WriteLine("编码结果: " + Program.Encode(stringArg.value));
                    }
                    else
                    {
                        Console.WriteLine("参数错误");
                    }
                }
                else
                {
                    Console.WriteLine("参数数量错误");
                }
            });
            AssignCommand("decode", (args) => {
                if (args.Count == 1)
                {
                    Argument stringArg = args.Find(x => x.pattern == "S");
                    if (stringArg != null)
                    {
                        Console.WriteLine("解码结果: " + Program.Decode(stringArg.value));
                    }
                    else
                    {
                        Console.WriteLine("参数错误");
                    }
                }
                else
                {
                    Console.WriteLine("参数数量错误");
                }
            });
        }

        /// <summary>
        /// 注册单个命令
        /// </summary>
        /// <param name="commandName">命令名</param>
        /// <param name="action">命令行为</param>
        public static void AssignCommand(string commandName, Action<List<Argument>> action)
        {
            commandDir.Add(commandName, action);
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令源文本</param>
        public static void HandleCommand(string command)
        {
            string[] commandEle = command.Split(' ');

            //"command -pattern arg" => "command"
            string commandName;
            if (commandEle.Length > 0)
            {
                commandName = commandEle[0].ToLower();
                //Console.WriteLine(commandName);
            }
            else
            {
                Console.WriteLine("输入不能为空");
                return;
            }

            List<Argument> args = new List<Argument>();

            //"command -pattern arg -pattern arg" => "-pattern arg ", "-pattern arg "
            foreach (Match match in Regex.Matches(command, commandArgMatchPattern))
            {
                string fullArg = match.Value;

                #region DEBUG
#if DEBUG
                Console.WriteLine("fullarg = " + fullArg);
#endif
                #endregion
                //"-pattern arg " => " pattern arg"
                //" pattern arg " => "pattern", "arg"
                string[] patternAndArg = fullArg.Split(new char[]{ ' '}, 2);
                #region DEBUG
#if DEBUG
                Console.WriteLine("patternAndArg: " + patternAndArg.Length);
#endif
                #endregion
                string pattern;
                string arg;
                if (patternAndArg.Length == 2)
                {
                    pattern = patternAndArg[0].Replace("-", "");
                    arg = patternAndArg[1].Replace("\"", "");
                    #region DEBUG
#if DEBUG
                    Console.WriteLine("pattern = " + pattern);
                    Console.WriteLine("arg = " + arg);
#endif
                    #endregion
                    args.Add(new Argument(pattern, arg));
                }
            }

            Action<List<Argument>> action;
            if(commandDir.TryGetValue(commandName, out action))
            {
                action(args);
            }
            else
            {
                Console.WriteLine("命令不存在");
            }
        }

        /// <summary>
        /// 命令参数类
        /// </summary>
        public class Argument
        {
            public string pattern;
            public string value;

            public Argument()
            {

            }

            public Argument(string pattern,string value)
            {
                this.pattern = pattern;
                this.value = value;
            }
        }
    }
}
