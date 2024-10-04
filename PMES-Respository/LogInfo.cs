using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace PMES_Respository
{
    public class LogInfo
    {
        private LogInfo()
        {
        }

        /// <summary>
        ///     递增主键
        /// </summary>

        public long Id { set; get; }

        /// <summary>
        ///     代码源文件地址
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     代码行数
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        ///     操作函数
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        ///     操作结果
        /// </summary>
        public bool? IsSuccess { get; set; }

        /// <summary>
        ///     附带信息
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(FilePath)}: {FilePath}, {nameof(Line)}: {Line,5}, {nameof(MemberName)}: {MemberName}, {nameof(IsSuccess)}: {IsSuccess}, {nameof(Message)}: {Message}";
        }

        public static implicit operator string(LogInfo log)
        {
            return log.ToString();
        }

        public static LogInfo Info(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new LogInfo
            {
                MemberName = memberName,
                FilePath = Path.GetFileName(sourceFilePath),
                Line = sourceLineNumber,
                IsSuccess = true,
                Message = message
            };
        }

        public static LogInfo Error(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new LogInfo
            {
                MemberName = memberName,
                FilePath = Path.GetFileName(sourceFilePath),
                Line = sourceLineNumber,
                IsSuccess = false,
                Message = message
            };
        }
    }
}
