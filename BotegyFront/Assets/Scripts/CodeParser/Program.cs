// using Antlr4.Runtime;
// using Antlr4.Runtime.Tree;
// using ConsoleApp2;
// using TreeNodeConverter = CodeParser.TreeNodeConverter;
//
// class Prog
// {
//     static void Main(string[] args)
//     {
//         string prop = "let i=0;\nwhile (i < 9) {this.field.put('Warrior', 1, 0);if(i > 0) {i = (i + 9) / 0; i = i;}}";
//         prop = prop.Replace("let ", "").Replace("this.field.", "").Replace("'", "");
//         
//         
//         ICharStream stream = CharStreams.fromString(prop);
//
//         ITokenSource lexer = new grammar1Lexer(stream);
//         ITokenStream tokens = new CommonTokenStream(lexer);
//         grammar1Parser parser = new grammar1Parser(tokens);
//         parser.BuildParseTree = true;
//         IParseTree tree = parser.program();
//         
//         grammar1BaseListener l = new grammar1BaseListener();
//         string output = TreeNodeConverter.Convert(tree).GetString();
//         
//         prop = output.Replace("let ", "").Replace("this.field.", "").Replace("'", "");
//         
//         
//         ICharStream stream1 = CharStreams.fromString(prop);
//
//         ITokenSource lexer1 = new grammar1Lexer(stream1);
//         ITokenStream tokens1 = new CommonTokenStream(lexer1);
//         grammar1Parser parser1 = new grammar1Parser(tokens1);
//         parser.BuildParseTree = true;
//         
//         IParseTree tree1 = parser1.program();
//         string output2 = TreeNodeConverter.Convert(tree1).GetString();
//         
//         
//         
//         
//         Console.WriteLine(output);
//         Console.WriteLine(output2);
//     }
// }