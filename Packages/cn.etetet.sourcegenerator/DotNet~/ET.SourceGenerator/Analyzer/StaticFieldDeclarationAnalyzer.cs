using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ET
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class StaticFieldDeclarationAnalyzer : DiagnosticAnalyzer
    {
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>ImmutableArray.Create(StaticFieldDeclarationAnalyzerRule.Rule);
        
        public override void Initialize(AnalysisContext context)
        {
            if (!AnalyzerGlobalSetting.EnableAnalyzer)
            {
                return;
            }
            
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(this.Analyzer, SymbolKind.NamedType);
        }
        
        private void Analyzer(SymbolAnalysisContext context)
        {
            if (!AnalyzerHelper.IsAssemblyNeedAnalyze(context.Compilation.AssemblyName, AnalyzeAssembly.All))
            {
                return;
            }

            if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
            {
                return;
            }
            
            var symbol = context.Symbol;
        
            // 获取符号的所有声明位置
            foreach (var location in symbol.Locations)
            {
                if (location.IsInSource)
                {
                    // 获取SyntaxTree和文件路径
                    var syntaxTree = location.SourceTree;
                    var filePath = syntaxTree.FilePath;
                    
                    //针对配置相对目录做屏蔽
                    filePath = filePath.Replace('\\', '/').Replace("//", "/");
                    if (AnalyzerGlobalSetting.EnableClassIgnoreDirNames.Any(dir => filePath.Contains(dir.Replace('\\', '/').Replace("//", "/"))))
                    {
                        return;
                    }
                
                    // // 获取行号信息
                    // var lineSpan = location.GetLineSpan();
                    // var lineNumber = lineSpan.StartLinePosition.Line + 1;
                    // var columnNumber = lineSpan.StartLinePosition.Character + 1;
                    //
                    // // 获取完整路径或文件名
                    // var fileName = Path.GetFileName(filePath);
                    // var directory = Path.GetDirectoryName(filePath);
                    
                
                    // // 使用文件路径
                    // Console.WriteLine($"File: {filePath}");
                    // Console.WriteLine($"Location: Line {lineNumber}, Column {columnNumber}");
                }
            }
            


            foreach (ISymbol? memberSymbol in namedTypeSymbol.GetMembers())
            {
                if (memberSymbol is IFieldSymbol { IsConst: false,IsStatic:true } or IPropertySymbol { IsStatic: true })
                {
                    bool hasAttr = memberSymbol.GetAttributes().Any(x => x.AttributeClass?.ToString() == Definition.StaticFieldAttribute);
                    if (!hasAttr)
                    {
                        ReportDiagnostic(memberSymbol);
                    }
                }
            }

            void ReportDiagnostic(ISymbol symbol)
            {
                foreach (SyntaxReference? declaringSyntaxReference in symbol.DeclaringSyntaxReferences)
                {
                    Diagnostic diagnostic = Diagnostic.Create(StaticFieldDeclarationAnalyzerRule.Rule, declaringSyntaxReference.GetSyntax()?.GetLocation(), symbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
        
    }
}

