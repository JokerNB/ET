using System.Collections.Generic;

namespace ET
{
    public static class AnalyzerGlobalSetting
    {
        /// <summary>
        /// 是否开启项目的所有分析器
        /// </summary>
        public static bool EnableAnalyzer = true;

        /// <summary>
        /// EnableClass特性忽略目录
        /// </summary>
        public static HashSet<string> EnableClassIgnoreDirNames = new HashSet<string>()
        {

        };
    }
}